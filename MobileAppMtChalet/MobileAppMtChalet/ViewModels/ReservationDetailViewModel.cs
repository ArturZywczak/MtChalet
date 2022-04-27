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
                SelectedRoomIndex = reservationData.RoomId - 1;
                SelectedBedCountIndex = reservationData.NumberOfPeople - 1;

                PrepareEditInfo();
            }
        }

        private Reservation reservationData;
        public Reservation ReservationData {
            get => reservationData;
            set {
                SetProperty(ref reservationData, value);
            }
        }
        //END

        private bool editMode = false;
        public bool EditMode {
            get => editMode;
            set {
                SetProperty(ref editMode, value);

                SetProperty(ref selectedRoomIndex, reservationData.RoomId - 1);
                SetProperty(ref selectedBedCountIndex, reservationData.NumberOfPeople - 1);

                EditModeInversed = !value;
                OnPropertyChanged();
            }
        }
        private bool editModeInv = true;
        public bool EditModeInversed {
            get => editModeInv;
            set {
                SetProperty(ref editModeInv, value);
                OnPropertyChanged();
            }
        }



        private int selectedRoomIndex;
        public int SelectedRoomIndex {
            get => selectedRoomIndex;
            set { 
                SetProperty(ref selectedRoomIndex, value);
                ChangeSelectedRoom();
            }
        }

        private int selectedBedCountIndex;
        public int SelectedBedCountIndex {
            get => selectedBedCountIndex;
            set => SetProperty(ref selectedBedCountIndex, value);
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

            
        }

        #region Commands Functions
        private async void OnCancel() {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave() { 

            EditedReservation editedReservation = new EditedReservation() {
                OldReservationId = reservationData.ReservationId,
                Name = reservationData.Name,
                Surname = reservationData.Surname,
                RoomId = SelectedRoomIndex + 1,
                NumberOfPeople = selectedBedCountIndex + 1,
                StartingDate = reservationData.StartingDate,
                EndingDate = reservationData.EndingDate,
                Phone = reservationData.Phone,
                Email = reservationData.Email,
                ExtraInfo = reservationData.ExtraInfo,
                EmployeeId = reservationData.EmployeeId,
                CreationDate = reservationData.CreationDate,
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

            await _mtChaletService.DeleteReservation(reservationData.ReservationId);
            await Shell.Current.GoToAsync("..");
        }
        #endregion

        #region Other Functions
        private void ChangeSelectedRoom() {
            if (Rooms.Count > 0) {
                AvaliableBeds.Clear();
                if (editMode)
                    for (int i = 1; i <= Rooms[selectedRoomIndex].RoomCap; i++) AvaliableBeds.Add(i); //here
                else
                    for (int i = 1; i <= Rooms[reservationData.RoomId - 1].RoomCap; i++) AvaliableBeds.Add(i); //here
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


        async void PrepareEditInfo() {
            try {
                IEnumerable<EditedReservation> edits = await _mtChaletService.GetEditReservationDetails(reservationData.ReservationId);
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
                        if (editsList.Last().Name != reservationData.Name)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Imię",
                            ChangedBefore = editsList.Last().Name,
                            ChangedAfter = reservationData.Name,
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().Surname != reservationData.Surname)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Nazwisko",
                            ChangedBefore = editsList.Last().Surname,
                            ChangedAfter = reservationData.Surname,
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().RoomId != reservationData.RoomId)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Nr pokoju",
                            ChangedBefore = editsList.Last().RoomId.ToString(),
                            ChangedAfter = reservationData.RoomId.ToString(),
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().NumberOfPeople != reservationData.NumberOfPeople)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Ilość osób w pokoju",
                            ChangedBefore = editsList.Last().NumberOfPeople.ToString(),
                            ChangedAfter = reservationData.NumberOfPeople.ToString(),
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().StartingDate != reservationData.StartingDate)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Datę przybycia",
                            ChangedBefore = editsList.Last().StartingDate.ToString(),
                            ChangedAfter = reservationData.StartingDate.ToString(),
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().EndingDate != reservationData.EndingDate)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Datę wyjazdu",
                            ChangedBefore = editsList.Last().EndingDate.ToString(),
                            ChangedAfter = reservationData.EndingDate.ToString(),
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().Phone != reservationData.Phone)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Nr telefonu",
                            ChangedBefore = editsList.Last().Phone,
                            ChangedAfter = reservationData.Phone,
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().Email != reservationData.Email)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Adres email",
                            ChangedBefore = editsList.Last().Email,
                            ChangedAfter = reservationData.Email,
                            Date = editsList.Last().EditDate
                        });

                    if (editsList.Last().ExtraInfo != reservationData.ExtraInfo)
                        EditDetailsList.Add(new EditHistoryDetail {
                            UserName = editsList.Last().EditedByEmployeeId,
                            ChangedType = "Dodatkowe informacje",
                            ChangedBefore = editsList.Last().ExtraInfo,
                            ChangedAfter = reservationData.ExtraInfo,
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
