//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Salt)
                .IsFixedLength()
                .HasMaxLength(10);		

            this.Property(t => t.LoginId)
                .IsRequired()
                .HasMaxLength(20);
                
            this.Property(t => t.Department)
                .HasMaxLength(50);
                
            this.Property(t => t.Phone)
                .HasMaxLength(20);
                
            this.Property(t => t.Email)
                .HasMaxLength(50);
                
            this.Property(t => t.FirstName)
                .HasMaxLength(20);
                
            this.Property(t => t.SecondName)
                .HasMaxLength(20);
                
            this.Property(t => t.Position)
                .HasMaxLength(20);
                
            this.Property(t => t.Language)
                .HasMaxLength(15);
                
            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.UserId).HasColumnName("UserID");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.PasswordRev).HasColumnName("PasswordRev");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.LoginId).HasColumnName("LoginID");
            this.Property(t => t.Department).HasColumnName("Department");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.CreatedDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdateDate");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.SecondName).HasColumnName("SecondName");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.Language).HasColumnName("Language");
        }
    }
}

