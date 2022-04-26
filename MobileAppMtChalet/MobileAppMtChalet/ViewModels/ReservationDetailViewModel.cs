using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {
    [QueryProperty(nameof(ReservationDataJson), "ReservationData")]
    [QueryProperty(nameof(UserDataJson), "UserData")]
    class ReservationDetailViewModel : BaseViewModel {

        #region Private & Public for binding values
        private readonly IMtChaletService _mtChaletService;

        //PASSING RESERVATION DATA
        private string reservationDataJson;
        public string ReservationDataJson {
            get => reservationDataJson;
            set {
                SetProperty(ref reservationDataJson, value);
                ReservationData = new Reservation(value);
            }
        }

        private Reservation reservationData;
        public Reservation ReservationData {
            get => reservationData;
            set {
                SetProperty(ref reservationData, value);
                if (Rooms.Count>0) LoadReservation();
            }
        }
        //END

        private string reservationId;
        public string ReservationId {
            get {
                return reservationId;
            }
            set {
                reservationId = value;
                //LoadReservationId(value);
            }
        }
        private string name;
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }
        private string surname;
        public string Surname {
            get => surname;
            set => SetProperty(ref surname, value);
        }
        private int roomID;
        public int RoomID {
            get => roomID;
            set {
                SetProperty(ref roomID, value);
                if (roomID != -1) {
                    ChangeSelectedRoom();
                }
            }
        }
        private int numberOfPeople;
        public int NumberOfPeople {
            get => numberOfPeople;
            set => SetProperty(ref numberOfPeople, value);
        }
        private DateTime startingDate;
        public DateTime StartingDate {
            get => startingDate;
            set => SetProperty(ref startingDate, value);
        }
        private DateTime endingDate;
        public DateTime EndingDate {
            get => endingDate;
            set => SetProperty(ref endingDate, value);
        }
        private string phone;
        public string Phone {
            get => phone;
            set => SetProperty(ref phone, value);
        }
        private string email;
        public string Email {
            get => email;
            set => SetProperty(ref email, value);
        }
        private string extraInfo;
        public string ExtraInfo {
            get => extraInfo;
            set => SetProperty(ref extraInfo, value);
        }
        private DateTime creationDate;
        public DateTime CreationDate {
            get => creationDate;
            set => SetProperty(ref creationDate, value);
        }
        private bool editMode;
        public bool EditMode {
            get => editMode;
            set {
                SetProperty(ref editMode, value);
                SelectedRoomID = RoomID - 1;
                SelectedBedCountID = NumberOfPeople - 1;
                EditModeInversed = !value;
                OnPropertyChanged();
            }
        }
        private bool editModeInv;
        public bool EditModeInversed {
            get => editModeInv;
            set {
                SetProperty(ref editModeInv, value);
                OnPropertyChanged();
            }
        }
        private string oldUserID;
        public string OldUserID {
            get {
                return oldUserID;
            }
            set {
                SetProperty(ref oldUserID, value);
            }
        }
        //terrible hack, bug fix
        private int selectedRoomID;
        public int SelectedRoomID {
            get => selectedRoomID;
            set { 
                SetProperty(ref selectedRoomID, value);
                if (roomID != -1) {
                    ChangeSelectedRoom();
                }
            }
        }

        private int selectedBedCountID;
        public int SelectedBedCountID {
            get => selectedBedCountID;
            set => SetProperty(ref selectedBedCountID, value);
        }
        #endregion
        # region Public Lists & Commands
        public ObservableCollection<int> RoomIDs { get; }
        public List<Room> Rooms { get; set; }
        public ObservableCollection<int> AvaliableBeds { get; }
        public ObservableCollection<EditHistoryDetail> EditDetailsList { get; }
        public Command EditReservationCommand { get; }
        public Command SaveEditCommand { get; }
        public Command CancelCommand { get; }
        public Command DeleteCommand { get; }
        #endregion
        public ReservationDetailViewModel(IMtChaletService mtChaletService) {

            RoomID = -1;
            _mtChaletService = mtChaletService;
            SaveEditCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            EditReservationCommand = new Command(OnEditReservation);
            DeleteCommand = new Command(OnDelete);

            RoomIDs = new ObservableCollection<int>();
            Rooms = new List<Room>();
            AvaliableBeds = new ObservableCollection<int>();
            EditDetailsList = new ObservableCollection<EditHistoryDetail>();

            PrepareRoomInfo();


            EditMode = false;
            EditModeInversed = true;
            

            
        }

        public void OnAppearing() {
            LoadReservation();
        }

        #region Commands Functions
        private async void OnCancel() {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave() { //TODO zamiast rezerwacji ma przygotować ReservationsEditHistory(zmienić nazwe?, ma dac stare id i dane pracownika)

            EditedReservation editedReservation = new EditedReservation() {
                OldReservationId = Int32.Parse(ReservationId),
                Name = name,
                Surname = surname,
                RoomId = SelectedRoomID + 1,
                NumberOfPeople = selectedBedCountID + 1,
                StartingDate = startingDate,
                EndingDate = endingDate,
                Phone = phone,
                Email = email,
                ExtraInfo = extraInfo,
                EmployeeId = oldUserID,
                CreationDate = creationDate,
                EditDate = DateTime.Now,
                EditedByEmployeeId = UserData.Auth0ID
                };

            await _mtChaletService.EditReservation(editedReservation);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private void OnEditReservation(object obj) {
            editMode = true;
            editModeInv = false;
        }
        private async void OnDelete() {

            await _mtChaletService.DeleteReservation(Int32.Parse(ReservationId));
            await Shell.Current.GoToAsync("..");
        }
        #endregion

        #region Other Functions
        private void ChangeSelectedRoom() {
            if (Rooms.Count > 0) {
                AvaliableBeds.Clear();
                if (editMode)
                    for (int i = 1; i <= Rooms[selectedRoomID].RoomCap; i++) AvaliableBeds.Add(i); //here
                else
                    for (int i = 1; i <= Rooms[roomID - 1].RoomCap; i++) AvaliableBeds.Add(i); //here
            }
        }
        async void PrepareRoomInfo() {
            try {
                IsBusy = true;
                var rooms = await _mtChaletService.GetRooms();
                foreach (var room in rooms) {
                    RoomIDs.Add(room.RoomId);
                    Rooms.Add(room);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            finally {
                IsBusy = false;
            }
        }
        public void LoadReservationId(string reservationId) {
            try {
                //var reservation = await _mtChaletService.GetReservation(reservationId);
                var reservation = new Reservation(reservationDataJson);
                Name = reservation.Name;
                Surname = reservation.Surname;
                RoomID = reservation.RoomId;
                NumberOfPeople = reservation.NumberOfPeople;
                StartingDate = reservation.StartingDate;
                EndingDate = reservation.EndingDate;
                Phone = reservation.Phone;
                Email = reservation.Email;
                ExtraInfo = reservation.ExtraInfo;
                CreationDate = reservation.CreationDate;

                ReservationId = reservation.ReservationId.ToString(); //added
                PrepareEditInfo();
            }
            catch (Exception) {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        public void LoadReservation() {
            

            var reservation = new Reservation(reservationDataJson);
            Name = reservation.Name;
            Surname = reservation.Surname;
            RoomID = reservation.RoomId;
            NumberOfPeople = reservation.NumberOfPeople;
            StartingDate = reservation.StartingDate;
            EndingDate = reservation.EndingDate;
            Phone = reservation.Phone;
            Email = reservation.Email;
            ExtraInfo = reservation.ExtraInfo;
            CreationDate = reservation.CreationDate;

            ReservationId = reservation.ReservationId.ToString(); //added

            //TODO If not allowed dont do it?
            PrepareEditInfo();
        }

        async void PrepareEditInfo() {
            try {
                IEnumerable<EditedReservation> edits = await _mtChaletService.GetEditReservationDetails(Int32.Parse(ReservationId));
                var editsList = edits.Reverse().ToList();

                for (int i = 0; i != editsList.Count-1; i++) { //-1 bo ostatni edit musi zostać porównany do aktualnej rezerwacji
                    if (editsList[i].OldReservationId == 0) ; //TODO HERE NEW

                    else { //porównaj do poprzedniego, jeśli się nie zgadza to dodaj do listy zmian
                        //to powinno być w jakiejś funkcji
                        if (editsList[i].Name != editsList[i - 1].Name) 
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i-1].EditedByEmployeeId,
                                ChangedType ="Imię",
                                ChangedBefore = editsList[i - 1].Name, 
                                ChangedAfter = editsList[i].Name, 
                                Date = editsList[i - 1].EditDate});

                        if (editsList[i].Surname != editsList[i - 1].Surname)
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i - 1].EditedByEmployeeId,
                                ChangedType = "Nazwisko",
                                ChangedBefore = editsList[i - 1].Surname,
                                ChangedAfter = editsList[i].Surname,
                                Date = editsList[i - 1].EditDate
                            });

                        if (editsList[i].RoomId != editsList[i - 1].RoomId)
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i - 1].EditedByEmployeeId,
                                ChangedType = "Nr pokoju",
                                ChangedBefore = editsList[i - 1].RoomId.ToString(),
                                ChangedAfter = editsList[i].RoomId.ToString(),
                                Date = editsList[i - 1].EditDate
                            });

                        if (editsList[i].NumberOfPeople != editsList[i - 1].NumberOfPeople)
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i - 1].EditedByEmployeeId,
                                ChangedType = "Ilość osób w pokoju",
                                ChangedBefore = editsList[i - 1].NumberOfPeople.ToString(),
                                ChangedAfter = editsList[i].NumberOfPeople.ToString(),
                                Date = editsList[i - 1].EditDate
                            });

                        if (editsList[i].StartingDate != editsList[i - 1].StartingDate)
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i - 1].EditedByEmployeeId,
                                ChangedType = "Datę przybycia",
                                ChangedBefore = editsList[i - 1].StartingDate.ToString(),
                                ChangedAfter = editsList[i].StartingDate.ToString(),
                                Date = editsList[i - 1].EditDate
                            });

                        if (editsList[i].EndingDate != editsList[i - 1].EndingDate)
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i - 1].EditedByEmployeeId,
                                ChangedType = "Datę wyjazdu",
                                ChangedBefore = editsList[i - 1].EndingDate.ToString(),
                                ChangedAfter = editsList[i].EndingDate.ToString(),
                                Date = editsList[i - 1].EditDate
                            });

                        if (editsList[i].Phone != editsList[i - 1].Phone)
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i - 1].EditedByEmployeeId,
                                ChangedType = "Nr telefonu",
                                ChangedBefore = editsList[i - 1].Phone,
                                ChangedAfter = editsList[i].Phone,
                                Date = editsList[i - 1].EditDate
                            });

                        if (editsList[i].Email != editsList[i - 1].Email)
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i - 1].EditedByEmployeeId,
                                ChangedType = "Adres email",
                                ChangedBefore = editsList[i - 1].Email,
                                ChangedAfter = editsList[i].Email,
                                Date = editsList[i - 1].EditDate
                            });

                        if (editsList[i].ExtraInfo != editsList[i - 1].ExtraInfo)
                            EditDetailsList.Add(new EditHistoryDetail {
                                UserName = editsList[i - 1].EditedByEmployeeId,
                                ChangedType = "Dodatkowe informacje",
                                ChangedBefore = editsList[i - 1].ExtraInfo,
                                ChangedAfter = editsList[i].ExtraInfo,
                                Date = editsList[i - 1].EditDate
                            });
                    }
                }

                { 
                        if (editsList.Last().Name != name)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Imię",
                            ChangedBefore = editsList.Last().Name,
                            ChangedAfter = name,
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().Surname != surname)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Nazwisko",
                            ChangedBefore = editsList.Last().Surname,
                            ChangedAfter = surname,
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().RoomId != roomID)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Nr pokoju",
                            ChangedBefore = editsList.Last().RoomId.ToString(),
                            ChangedAfter = roomID.ToString(),
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().NumberOfPeople != numberOfPeople)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Ilość osób w pokoju",
                            ChangedBefore = editsList.Last().NumberOfPeople.ToString(),
                            ChangedAfter = numberOfPeople.ToString(),
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().StartingDate != startingDate)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Datę przybycia",
                            ChangedBefore = editsList.Last().StartingDate.ToString(),
                            ChangedAfter = startingDate.ToString(),
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().EndingDate != endingDate)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Datę wyjazdu",
                            ChangedBefore = editsList.Last().EndingDate.ToString(),
                            ChangedAfter = endingDate.ToString(),
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().Phone != phone)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Nr telefonu",
                            ChangedBefore = editsList.Last().Phone,
                            ChangedAfter = phone,
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().Email != email)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Adres email",
                            ChangedBefore = editsList.Last().Email,
                            ChangedAfter = email,
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().ExtraInfo != extraInfo)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Dodatkowe informacje",
                            ChangedBefore = editsList.Last().ExtraInfo,
                            ChangedAfter = extraInfo,
                            Date = editsList.Last().EditDate
                        });


                } //paskudztwo


            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }

        }
        #endregion
    }
}
