using Garden.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Data
{
    public class SeedData
    {
         public static void Initialize(IServiceProvider serviceProvider)
         {
            using(ApplicationDbContext context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                #region 기본 타입(BaseType)
                if (context.BaseType.Any() == false)
                {
                    context.BaseType.AddRange(
                        new BaseType { Id="GARDEN_TYPE", Name = "정원 타입", Description = "정원 타입", IsSubTypeEditable = true },
                        new BaseType { Id="GARDEN_MANAGER_ROLE_TYPE", Name = "정원 관리자 역할 타입", Description = "정원 관리자 역할 타입", IsSubTypeEditable = true },
                        new BaseType { Id="GARDEN_TASK_TYPE", Name = "정원 업무 타입", Description = "정원 업무 타입", IsSubTypeEditable = true },
                        new BaseType { Id="GARDEN_TASK_TIME_TYPE", Name = "정원 업무 시간 타입", Description = "정원 업무 시간 타입", IsSubTypeEditable = true },
                        new BaseType { Id="LOG_TYPE", Name = "로그 타입", Description = "로그 타입", IsSubTypeEditable = false }
                        //new BaseType { Id="GARDEN_FEE_TYPE", Name= "회비 타입", Description = "회비 타입", IsSubTypeEditable = true },
                        //new BaseType { Id="GARDEN_FEE_DISCOUNT_TYPE", Name= "회비 할인 타입", Description = "회비 할인 타입", IsSubTypeEditable = true }
                        );
                    context.SaveChanges();
                }
                #endregion

                #region 서브 타입(BaseSubType)
                if(context.BaseSubType.Any() == false)
                {
                    context.BaseSubType.AddRange(
                        //정원 타입
                        new BaseSubType { Id="GARDEN_TYPE_1", BaseTypeId = "GARDEN_TYPE", Name="학원", Description="학원"},
                        new BaseSubType { Id="GARDEN_TYPE_2", BaseTypeId = "GARDEN_TYPE", Name="기타", Description="기타"},
                        //정원 관리자 역할 타입
                        new BaseSubType { Id = "GARDEN_MANAGER_ROLE_TYPE_1", BaseTypeId = "GARDEN_MANAGER_ROLE_TYPE", Name="마스터 정원사", Description="마스터 정원사" },
                        new BaseSubType { Id = "GARDEN_MANAGER_ROLE_TYPE_2", BaseTypeId = "GARDEN_MANAGER_ROLE_TYPE", Name="수석 정원사", Description = "수석 정원사"},
                        new BaseSubType { Id = "GARDEN_MANAGER_ROLE_TYPE_3", BaseTypeId = "GARDEN_MANAGER_ROLE_TYPE", Name="정원사", Description ="정원사"},
                        new BaseSubType { Id = "GARDEN_MANAGER_ROLE_TYPE_4", BaseTypeId = "GARDEN_MANAGER_ROLE_TYPE", Name ="Flower", Description = "Flower"},
                        //정원 업무 타입
                        new BaseSubType { Id = "GARDEN_TASK_TYPE_1", BaseTypeId = "GARDEN_TASK_TYPE", Name="수업", Description = "수업"},
                        new BaseSubType { Id = "GARDEN_TASK_TYPE_2", BaseTypeId = "GARDEN_TASK_TYPE", Name = "수강", Description = "수강" },
                        new BaseSubType { Id = "GARDEN_TASK_TYPE_3", BaseTypeId = "GARDEN_TASK_TYPE", Name ="기타", Description = "기타"},
                        //정원 업무 시간 타입
                        new BaseSubType { Id = "GARDEN_TASK_TIME_TYPE_1", BaseTypeId = "GARDEN_TASK_TIME_TYPE", Name="수업", Description = "수업"},
                        new BaseSubType { Id = "GARDEN_TASK_TIME_TYPE_2", BaseTypeId = "GARDEN_TASK_TIME_TYPE", Name ="수강", Description = "수강"},
                        new BaseSubType { Id = "GARDEN_TASK_TIME_TYPE_3", BaseTypeId = "GARDEN_TASK_TIME_TYPE", Name = "기타", Description = "기타" },
                        //회비 타입
                        //new BaseSubType { Id = "GARDEN_FEE_TYPE_1", BaseTypeId = "GARDEN_FEE_TYPE", Name = "1개월", Description = "" },
                        //new BaseSubType { Id = "GARDEN_FEE_TYPE_2", BaseTypeId = "GARDEN_FEE_TYPE", Name = "3개월", Description = "" },
                        //new BaseSubType { Id = "GARDEN_FEE_TYPE_3", BaseTypeId = "GARDEN_FEE_TYPE", Name = "6개월", Description = "" },
                        ////회비 할인 타입
                        //new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_1", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "첫달 홈페이지 수강후기 작성", Description = "" },
                        //new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_2", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "수강한지 1년", Description = "" },
                        //new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_3", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "친구 데려오기", Description = "" },
                        //new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_4", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "리뷰 작성", Description = "" },
                        //new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_5", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "원데이 클래스", Description = "" },
                        //new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_6", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "피아노 3인 그룹", Description = "" },
                        //로그 타입
                        new BaseSubType { Id = "LOG_TYPE_1", BaseTypeId = "LOG_TYPE", Name = "시스템 로그", Description = "시스템 로그" },
                        new BaseSubType { Id = "LOG_TYPE_2", BaseTypeId = "LOG_TYPE", Name = "작업 로그", Description = "작업 로그" }
                        );
                    context.SaveChanges();
                }
                #endregion

            }
        }

        #region 역할 추가
        public static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Admin";
                role.Description = "Admin";
                role.Grade = 0;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if(!roleManager.RoleExistsAsync("SuperManager").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "SuperManager";
                role.Description = "SuperManager";
                role.Grade = 1;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Manager").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Manager";
                role.Description = "Manager";
                role.Grade = 2;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "User";
                role.Description = "User";
                role.Grade = 3;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
        #endregion

        #region 관리자 계정 추가
        // system 계정 추가
        public static void SeedSystemAccount(UserManager<ApplicationUser> userManager)
        {
            if(userManager.FindByNameAsync("SYSTEM").Result == null)
            {
                ApplicationUser user = new ApplicationUser();

                user.UserName = "SYSTEM";
                user.Email = "system@system.kr";
                user.Name = "관리자";
                user.IsActive = true;
                user.EmailConfirmed = true;


                IdentityResult result = userManager.CreateAsync(user, "emth022944W!").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }
        #endregion

        #region 회비, 회비 할인 추가
        public static void SeedFeeTypes(IServiceProvider serviceProvider)
        {
            using(ApplicationDbContext context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                //회비 타입 존재 체크
                if(context.BaseType.Find("GARDEN_FEE_TYPE") == null)
                {
                    context.BaseType.Add(
                        new BaseType { Id = "GARDEN_FEE_TYPE", Name = "회비 타입", Description = "회비 타입", IsSubTypeEditable = true }
                        );

                    context.SaveChanges();

                    context.BaseSubType.AddRange(
                       //회비 타입
                       new BaseSubType { Id = "GARDEN_FEE_TYPE_1", BaseTypeId = "GARDEN_FEE_TYPE", Name = "1개월", Description = "1" },
                       new BaseSubType { Id = "GARDEN_FEE_TYPE_2", BaseTypeId = "GARDEN_FEE_TYPE", Name = "3개월", Description = "3" },
                       new BaseSubType { Id = "GARDEN_FEE_TYPE_3", BaseTypeId = "GARDEN_FEE_TYPE", Name = "6개월", Description = "5" }
                   );
                    context.SaveChanges();
                }

                //회비 할인 타입 존재 체크
                if(context.BaseType.Find("GARDEN_FEE_DISCOUNT_TYPE") == null)
                {
                    context.BaseType.Add(
                       new BaseType { Id = "GARDEN_FEE_DISCOUNT_TYPE", Name = "회비 할인 타입", Description = "회비 할인 타입", IsSubTypeEditable = true }
                       );

                    context.SaveChanges();

                    context.BaseSubType.AddRange(

                        //회비 할인 타입
                        new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_0", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "없음", Description = "" },
                        new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_1", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "첫달 홈페이지 수강후기 작성", Description = "" },
                        new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_2", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "수강한지 1년", Description = "" },
                        new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_3", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "친구 데려오기", Description = "" },
                        new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_4", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "리뷰 작성", Description = "" },
                        new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_5", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "원데이 클래스", Description = "" },
                        new BaseSubType { Id = "GARDEN_FEE_DISCOUNT_TYPE_6", BaseTypeId = "GARDEN_FEE_DISCOUNT_TYPE", Name = "피아노 3인 그룹", Description = "" }

                   );
                    context.SaveChanges();
                }
            }
        }
        #endregion
    }
}
