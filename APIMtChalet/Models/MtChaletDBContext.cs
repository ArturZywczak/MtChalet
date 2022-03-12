using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace APIMtChalet.Models {
    public class MtChaletDBContext : DbContext {
        public MtChaletDBContext(DbContextOptions<MtChaletDBContext> options)
            : base(options) {
        }

        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Employee>(entity => {
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Auth0Id)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Auth0ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reservation>(entity => {
                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EndingDate).HasColumnType("date");

                entity.Property(e => e.ExtraInfo)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.StartingDate).HasColumnType("date");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReservationsEditHistory>(entity => {
                entity.HasKey(e => e.ReservationEditId)
                    .HasName("PK_ReservationEditHistory");

                entity.ToTable("ReservationsEditHistory");

                entity.Property(e => e.ReservationEditId).HasColumnName("ReservationEditID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditedByEmployeeId).HasColumnName("EditedByEmployeeID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EndingDate).HasColumnType("date");

                entity.Property(e => e.ExtraInfo)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NewReservationId).HasColumnName("NewReservationID");

                entity.Property(e => e.OldReservationId).HasColumnName("OldReservationID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.StartingDate).HasColumnType("date");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Room>(entity => {
                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.RoomNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

        }

    }
}
