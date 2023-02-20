namespace DigitusDemo.Migrations
{
    using DigitusDemo.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;

    internal sealed class Configuration : DbMigrationsConfiguration<DigitusDemo.Data.DigitusDemoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DigitusDemo.Data.DigitusDemoContext context)
        {
            var users = new List<User>
            {
                new User { Name = "Hakan",  Surname = "Kalkan", Email="hakankalkan@digitus.com",Password="123456".GetHashCode().ToString(),IsAdmin=true,IsValid=true,ValidationCode=new Random().Next().ToString(), RegisteredDate=DateTime.Parse("17.02.2023 20:00"),IsOnline=false,RegCodeSent=true,RegCodeSentTime=DateTime.Parse("17.02.2023") }
            };
            users.ForEach(s => context.Users.AddOrUpdate(p => p.UserId, s));
            
            context.SaveChanges();
        }
    }
}
