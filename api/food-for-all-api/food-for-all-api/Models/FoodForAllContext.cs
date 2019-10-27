using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace food_for_all_api.Models
{
    public partial class FoodForAllContext : DbContext
    {
        public FoodForAllContext()
        {
        }

        public FoodForAllContext(DbContextOptions<FoodForAllContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CalificationStock> CalificationStock { get; set; }
        public virtual DbSet<CalificationUser> CalificationUser { get; set; }
        public virtual DbSet<Denounced> Denounced { get; set; }
        public virtual DbSet<EventLog> EventLog { get; set; }
        public virtual DbSet<EventLogType> EventLogType { get; set; }
        public virtual DbSet<GlobalSetting> GlobalSetting { get; set; }
        public virtual DbSet<Institution> Institution { get; set; }
        public virtual DbSet<ListBlack> ListBlack { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<StockAvailable> StockAvailable { get; set; }
        public virtual DbSet<StockComment> StockComment { get; set; }
        public virtual DbSet<StockImage> StockImage { get; set; }
        public virtual DbSet<StockReceived> StockReceived { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<TypeMessage> TypeMessage { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot iConfigurationRoot = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

                string connectionString = iConfigurationRoot.GetConnectionString("ProductionContext");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CalificationStock>(entity =>
            {
                entity.HasOne(d => d.IdStockNavigation)
                    .WithMany(p => p.CalificationStock)
                    .HasForeignKey(d => d.IdStock)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Calificat__IdSto__6F7569AA");

                entity.HasOne(d => d.IdUserCalificationNavigation)
                    .WithMany(p => p.CalificationStock)
                    .HasForeignKey(d => d.IdUserCalification)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Calificat__IdUse__70698DE3");
            });

            modelBuilder.Entity<CalificationUser>(entity =>
            {
                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.CalificationUserIdUserNavigation)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Calificat__IdUse__5986288B");

                entity.HasOne(d => d.IdUserCalificationNavigation)
                    .WithMany(p => p.CalificationUserIdUserCalificationNavigation)
                    .HasForeignKey(d => d.IdUserCalification)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Calificat__IdUse__5A7A4CC4");
            });

            modelBuilder.Entity<Denounced>(entity =>
            {
                entity.Property(e => e.Reason).IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.DenouncedIdUserNavigation)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Denounced__IdUse__5D56B96F");

                entity.HasOne(d => d.IdUserAccuserNavigation)
                    .WithMany(p => p.DenouncedIdUserAccuserNavigation)
                    .HasForeignKey(d => d.IdUserAccuser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Denounced__IdUse__5E4ADDA8");
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.Property(e => e.Controller)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Host)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HttpMethod)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Message).IsUnicode(false);

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEventLogTypeNavigation)
                    .WithMany(p => p.EventLog)
                    .HasForeignKey(d => d.IdEventLogType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventLog__IdEven__0D05CC91");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.EventLog)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__EventLog__IdUser__0C11A858");
            });

            modelBuilder.Entity<EventLogType>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlobalSetting>(entity =>
            {
                entity.Property(e => e.Property)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Institution>(entity =>
            {
                entity.Property(e => e.Activity)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Commune)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Rut)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ListBlack>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OneSignalPlayerId).IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ListBlack)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ListBlack__IdUse__61274A53");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Location__IdUser__6403B6FE");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Message1)
                    .IsRequired()
                    .HasColumnName("Message")
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTypeMessageNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.IdTypeMessage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__IdTypeM__7EB7AD3A");

                entity.HasOne(d => d.IdUserReceivedNavigation)
                    .WithMany(p => p.MessageIdUserReceivedNavigation)
                    .HasForeignKey(d => d.IdUserReceived)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__IdUserR__009FF5AC");

                entity.HasOne(d => d.IdUserSendNavigation)
                    .WithMany(p => p.MessageIdUserSendNavigation)
                    .HasForeignKey(d => d.IdUserSend)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__IdUserS__7FABD173");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceImage).IsUnicode(false);

                entity.HasOne(d => d.IdProductTypeNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.IdProductType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__IdProdu__68C86C1B");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceImage).IsUnicode(false);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.Property(e => e.DateOfAdmission).HasColumnType("datetime");

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.Observation).IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock__IdProduct__6C98FCFF");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock__IdUser__6BA4D8C6");
            });

            modelBuilder.Entity<StockAvailable>(entity =>
            {
                entity.Property(e => e.DateOfAdmission).HasColumnType("datetime");

                entity.HasOne(d => d.IdStockNavigation)
                    .WithMany(p => p.StockAvailable)
                    .HasForeignKey(d => d.IdStock)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockAvai__IdSto__7345FA8E");
            });

            modelBuilder.Entity<StockComment>(entity =>
            {
                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.IdStockNavigation)
                    .WithMany(p => p.StockComment)
                    .HasForeignKey(d => d.IdStock)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockComm__IdSto__037C6257");

                entity.HasOne(d => d.IdTypeMessageNavigation)
                    .WithMany(p => p.StockComment)
                    .HasForeignKey(d => d.IdTypeMessage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockComm__IdTyp__04708690");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.StockComment)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockComm__IdUse__0564AAC9");
            });

            modelBuilder.Entity<StockImage>(entity =>
            {
                entity.Property(e => e.ReferenceImage)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.IdStockNavigation)
                    .WithMany(p => p.StockImage)
                    .HasForeignKey(d => d.IdStock)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockImag__IdSto__76226739");
            });

            modelBuilder.Entity<StockReceived>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.IdStockNavigation)
                    .WithMany(p => p.StockReceived)
                    .HasForeignKey(d => d.IdStock)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockRece__IdSto__78FED3E4");

                entity.HasOne(d => d.IdUserBeneficiaryNavigation)
                    .WithMany(p => p.StockReceived)
                    .HasForeignKey(d => d.IdUserBeneficiary)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StockRece__IdUse__79F2F81D");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.Host)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token1)
                    .IsRequired()
                    .HasColumnName("Token")
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Token)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Token__IdUser__0FE2393C");
            });

            modelBuilder.Entity<TypeMessage>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DateOfAdmission).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OneSignalPlayerId).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Photo).IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdInstitutionNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.IdInstitution)
                    .HasConstraintName("FK__User__IdInstitut__56A9BBE0");

                entity.HasOne(d => d.IdUserTypeNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.IdUserType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__IdUserType__55B597A7");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
