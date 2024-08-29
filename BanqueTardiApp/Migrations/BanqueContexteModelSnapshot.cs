﻿// <auto-generated />
using System;
using BanqueTardiApp.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BanqueTardiApp.Migrations
{
    [DbContext(typeof(BanqueContexte))]
    partial class BanqueContexteModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("BanqueTardiApp.Models.Client", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("CodePostal")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateNaissance")
                        .HasColumnType("TEXT");

                    b.Property<int>("NbDecouverts")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<string>("NomParent")
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TelephoneParent")
                        .HasColumnType("TEXT");

                    b.HasKey("Code");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("BanqueTardiApp.Models.CompteBancaire", b =>
                {
                    b.Property<string>("NumeroCompte")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientCode")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Solde")
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeCompte")
                        .HasColumnType("INTEGER");

                    b.HasKey("NumeroCompte");

                    b.HasIndex("ClientCode");

                    b.ToTable("ComptesBancaires");
                });

            modelBuilder.Entity("BanqueTardiApp.Models.Transaction", b =>
                {
                    b.Property<int>("Identifiant")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOperation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Libelle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Montant")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroCompte")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeOperation")
                        .HasColumnType("INTEGER");

                    b.HasKey("Identifiant");

                    b.HasIndex("NumeroCompte");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BanqueTardiApp.Models.CompteBancaire", b =>
                {
                    b.HasOne("BanqueTardiApp.Models.Client", "Client")
                        .WithMany("Comptes")
                        .HasForeignKey("ClientCode")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Client");
                });

            modelBuilder.Entity("BanqueTardiApp.Models.Transaction", b =>
                {
                    b.HasOne("BanqueTardiApp.Models.CompteBancaire", "CompteBancaire")
                        .WithMany("Transactions")
                        .HasForeignKey("NumeroCompte")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompteBancaire");
                });

            modelBuilder.Entity("BanqueTardiApp.Models.Client", b =>
                {
                    b.Navigation("Comptes");
                });

            modelBuilder.Entity("BanqueTardiApp.Models.CompteBancaire", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
