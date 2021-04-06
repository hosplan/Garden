using Garden.Models;
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
                        new BaseType { Name = "정원 타입", Description = "정원 타입", IsSubTypeEditable = true },
                        new BaseType { Name = "정원 관리자 역할 타입", Description = "정원 관리자 역할 타입", IsSubTypeEditable = true },
                        new BaseType { Name = "정원 업무 타입", Description = "정원 업무 타입", IsSubTypeEditable = true },
                        new BaseType { Name = "정원 업무 시간 타입", Description = "정원 업무 시간 타입", IsSubTypeEditable = true },
                        new BaseType { Name = "로그 타입", Description = "로그 타입", IsSubTypeEditable = false }
                        );
                    context.SaveChanges();
                }
                #endregion

                #region 서브 타입(BaseSubType)
                if(context.BaseSubType.Any() == false)
                {
                    context.BaseSubType.AddRange(
                        //정원 타입
                        new BaseSubType { BaseTypeId = 1, Name="학원", Description="학원"},
                        new BaseSubType { BaseTypeId = 1, Name="기타", Description="기타"},
                        //정원 관리자 역할 타입
                        new BaseSubType { BaseTypeId = 2, Name="마스터 정원사", Description="마스터 정원사" },
                        new BaseSubType { BaseTypeId = 2, Name="수석 정원사", Description = "수석 정원사"},
                        new BaseSubType { BaseTypeId = 2, Name="정원사", Description ="정원사"},
                        new BaseSubType { BaseTypeId = 2, Name ="Flower", Description = "Flower"},
                        //정원 업무 타입
                        new BaseSubType { BaseTypeId = 3, Name="수업", Description = "수업"},
                        new BaseSubType { BaseTypeId = 3, Name = "수강", Description = "수강" },
                        new BaseSubType { BaseTypeId = 3, Name ="기타", Description = "기타"},
                        //정원 업무 시간 타입
                        new BaseSubType { BaseTypeId = 4, Name="수업", Description = "수업"},
                        new BaseSubType { BaseTypeId = 4, Name ="수강", Description = "수강"},
                        new BaseSubType { BaseTypeId = 4, Name = "기타", Description = "기타" },
                        //로그 타입
                        new BaseSubType { BaseTypeId = 5, Name = "시스템 로그", Description = "시스템 로그" },
                        new BaseSubType { BaseTypeId = 5, Name = "작업 로그", Description = "작업 로그" }
                        );
                    context.SaveChanges();
                }
                #endregion

            }
        }
    }
}
