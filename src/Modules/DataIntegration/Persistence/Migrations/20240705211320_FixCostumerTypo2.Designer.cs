﻿// <auto-generated />
using BIManagement.Modules.DataIntegration.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    [DbContext(typeof(DataIntegrationDbContext))]
    [Migration("20240705211320_FixCostumerTypo2")]
    partial class FixCostumerTypo2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dataIntegration")
                .HasAnnotation("ProductVersion", "9.0.0-preview.5.24306.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BIManagement.Modules.DataIntegration.Domain.DatabaseConnection.CustomerDbConnectionConfiguration", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConnectionString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Provider")
                        .HasColumnType("int");

                    b.HasKey("CustomerId");

                    b.ToTable("DatabaseConnectionConfigurations", "dataIntegration");
                });

            modelBuilder.Entity("BIManagement.Modules.DataIntegration.Domain.DbModelling.CustomerDbModel", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DbModel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("CustomerDbModels", "dataIntegration");
                });

            modelBuilder.Entity("BIManagement.Modules.DataIntegration.Domain.Mapping.SchemaMapping", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TargetDbTableId")
                        .HasColumnType("int");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<string>("Mapping")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId", "TargetDbTableId");

                    b.ToTable("SchemaMappings", "dataIntegration");
                });

            modelBuilder.Entity("BIManagement.Modules.DataIntegration.Domain.Mapping.TargetDbTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableModel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TargetDbTables", "dataIntegration");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Schema = "dbo",
                            TableModel = "{\"name\":\"Employees\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"Id\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"PersonalID\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"ExternalId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"FirstName\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":false}},{\"name\":\"Lastname\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":false}}],\"description\":null}",
                            TableName = "Employees"
                        },
                        new
                        {
                            Id = 2,
                            Schema = "dbo",
                            TableModel = "{\"name\":\"Workplaces\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"Id\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"Label\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":false}},{\"name\":\"Name\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":true}}],\"description\":null}",
                            TableName = "Workplaces"
                        },
                        new
                        {
                            Id = 3,
                            Schema = "dbo",
                            TableModel = "{\"name\":\"WorkReports\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"ID\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"ID\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"OrderId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":true}},{\"name\":\"ProductionOperationId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":true}},{\"name\":\"Quantity\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Decimal\",\"isNullable\":false}},{\"name\":\"ExpectedTime\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Decimal\",\"isNullable\":false}},{\"name\":\"DateTime\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Datetime\",\"isNullable\":false}},{\"name\":\"ProductType\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":true}},{\"name\":\"WorkerId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":true}},{\"name\":\"WorkPlaceId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":true}}],\"description\":null}",
                            TableName = "WorkReports"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}