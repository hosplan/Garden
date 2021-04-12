using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garden.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<double>(type: "float", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSubTypeEditable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseSubType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseTypeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseSubType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseSubType_BaseType_BaseTypeId",
                        column: x => x.BaseTypeId,
                        principalTable: "BaseType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GardenSpace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActivate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenSpace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenSpace_BaseSubType_SubTypeId",
                        column: x => x.SubTypeId,
                        principalTable: "BaseSubType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GardenAttachMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GardenId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenAttachMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenAttachMap_Attachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GardenAttachMap_GardenSpace_GardenId",
                        column: x => x.GardenId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GardenRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GardenId = table.Column<int>(type: "int", nullable: true),
                    SubTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenRole_BaseSubType_SubTypeId",
                        column: x => x.SubTypeId,
                        principalTable: "BaseSubType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GardenRole_GardenSpace_GardenId",
                        column: x => x.GardenId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GardenTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTypeId = table.Column<int>(type: "int", nullable: true),
                    IsActivate = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GardenSpaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenTask_BaseSubType_SubTypeId",
                        column: x => x.SubTypeId,
                        principalTable: "BaseSubType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GardenTask_GardenSpace_GardenSpaceId",
                        column: x => x.GardenSpaceId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GardenTaskAttachMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GardenId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenTaskAttachMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenTaskAttachMap_Attachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GardenTaskAttachMap_GardenSpace_GardenId",
                        column: x => x.GardenId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GardenUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActivate = table.Column<bool>(type: "bit", nullable: false),
                    GardenSpaceId = table.Column<int>(type: "int", nullable: true),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GardenUser_GardenSpace_GardenSpaceId",
                        column: x => x.GardenSpaceId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GardenWorkDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GardenSpaceId = table.Column<int>(type: "int", nullable: true),
                    IsMon = table.Column<bool>(type: "bit", nullable: false),
                    IsTue = table.Column<bool>(type: "bit", nullable: false),
                    IsWed = table.Column<bool>(type: "bit", nullable: false),
                    IsThu = table.Column<bool>(type: "bit", nullable: false),
                    IsFri = table.Column<bool>(type: "bit", nullable: false),
                    IsSat = table.Column<bool>(type: "bit", nullable: false),
                    IsSun = table.Column<bool>(type: "bit", nullable: false),
                    SubTypeId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenWorkDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenWorkDay_BaseSubType_SubTypeId",
                        column: x => x.SubTypeId,
                        principalTable: "BaseSubType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GardenWorkDay_GardenSpace_GardenSpaceId",
                        column: x => x.GardenSpaceId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GardenWorkTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GardenSpaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenWorkTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenWorkTime_GardenSpace_GardenSpaceId",
                        column: x => x.GardenSpaceId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GardenUserTaskMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GardenManagerId = table.Column<int>(type: "int", nullable: true),
                    GardenUserId = table.Column<int>(type: "int", nullable: true),
                    GardenTaskId = table.Column<int>(type: "int", nullable: true),
                    GardenWorkTimeId = table.Column<int>(type: "int", nullable: true),
                    GardenWorkDayId = table.Column<int>(type: "int", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    TaskWeek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenUserTaskMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenUserTaskMap_GardenTask_GardenTaskId",
                        column: x => x.GardenTaskId,
                        principalTable: "GardenTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GardenUserTaskMap_GardenUser_GardenManagerId",
                        column: x => x.GardenManagerId,
                        principalTable: "GardenUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GardenUserTaskMap_GardenUser_GardenUserId",
                        column: x => x.GardenUserId,
                        principalTable: "GardenUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GardenUserTaskMap_GardenWorkDay_GardenWorkDayId",
                        column: x => x.GardenWorkDayId,
                        principalTable: "GardenWorkDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GardenUserTaskMap_GardenWorkTime_GardenWorkTimeId",
                        column: x => x.GardenWorkTimeId,
                        principalTable: "GardenWorkTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseSubType_BaseTypeId",
                table: "BaseSubType",
                column: "BaseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenAttachMap_AttachmentId",
                table: "GardenAttachMap",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenAttachMap_GardenId",
                table: "GardenAttachMap",
                column: "GardenId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenRole_GardenId",
                table: "GardenRole",
                column: "GardenId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenRole_SubTypeId",
                table: "GardenRole",
                column: "SubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenSpace_SubTypeId",
                table: "GardenSpace",
                column: "SubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenTask_GardenSpaceId",
                table: "GardenTask",
                column: "GardenSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenTask_SubTypeId",
                table: "GardenTask",
                column: "SubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenTaskAttachMap_AttachmentId",
                table: "GardenTaskAttachMap",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenTaskAttachMap_GardenId",
                table: "GardenTaskAttachMap",
                column: "GardenId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenUser_GardenSpaceId",
                table: "GardenUser",
                column: "GardenSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenUser_UserId",
                table: "GardenUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenUserTaskMap_GardenManagerId",
                table: "GardenUserTaskMap",
                column: "GardenManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenUserTaskMap_GardenTaskId",
                table: "GardenUserTaskMap",
                column: "GardenTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenUserTaskMap_GardenUserId",
                table: "GardenUserTaskMap",
                column: "GardenUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenUserTaskMap_GardenWorkDayId",
                table: "GardenUserTaskMap",
                column: "GardenWorkDayId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenUserTaskMap_GardenWorkTimeId",
                table: "GardenUserTaskMap",
                column: "GardenWorkTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenWorkDay_GardenSpaceId",
                table: "GardenWorkDay",
                column: "GardenSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenWorkDay_SubTypeId",
                table: "GardenWorkDay",
                column: "SubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenWorkTime_GardenSpaceId",
                table: "GardenWorkTime",
                column: "GardenSpaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "GardenAttachMap");

            migrationBuilder.DropTable(
                name: "GardenRole");

            migrationBuilder.DropTable(
                name: "GardenTaskAttachMap");

            migrationBuilder.DropTable(
                name: "GardenUserTaskMap");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "GardenTask");

            migrationBuilder.DropTable(
                name: "GardenUser");

            migrationBuilder.DropTable(
                name: "GardenWorkDay");

            migrationBuilder.DropTable(
                name: "GardenWorkTime");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "GardenSpace");

            migrationBuilder.DropTable(
                name: "BaseSubType");

            migrationBuilder.DropTable(
                name: "BaseType");
        }
    }
}
