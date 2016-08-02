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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DIS.Data.DataContract {
    public class User {
        public User() {
            this.UserHeadQuarters = new List<UserHeadQuarter>();
            this.Roles = new List<Role>();
        }

        public int UserId { get; set; }
        public string Password { get; set; }
        public int PasswordRev { get; set; }
        public string Salt { get; set; }
        public string LoginId { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public string Language { get; set; }
        public ICollection<UserHeadQuarter> UserHeadQuarters { get; set; }
        public ICollection<Role> Roles { get; set; }

        public void AddRole(Role role) {
            role.ActionCode = Constants.ActionCode.Inserted;
            this.Roles.Add(role);
        }

        public void RemoveRole(Role role) {
            role.ActionCode = Constants.ActionCode.Deleted;
        }

        [NotMapped]
        public PasswordVersion PasswordVersion {
            get { return (PasswordVersion)PasswordRev; }
            set { PasswordRev = (int)value; }
        }

        [NotMapped]
        public Role Role {
            get {
                return this.Roles.Count > 0 ? this.Roles.Single() : null;
            }
        }

        [NotMapped]
        public string RoleName {
            get {
                return this.Role.RoleName;
            }
        }

        [NotMapped]
        public string LocalCreatedDate
        {
            get
            {
                DateTimeOffset offset = new DateTimeOffset(CreatedDate, TimeZoneInfo.Utc.GetUtcOffset(CreatedDate));
                return offset.LocalDateTime.ToString();
            }
        }
    }
}

