using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TestClient.Data;

namespace TestClient.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170330011504_CreateWebServiceTable")]
    partial class CreateWebServiceTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("TestClient.Data.Model.SettingQueryResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LastSearchFilter");

                    b.Property<string>("LastSearchResult");

                    b.HasKey("Id");

                    b.ToTable("SettingQueryResults");
                });

            modelBuilder.Entity("TestClient.Data.Model.WebService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("WebService");
                });
        }
    }
}
