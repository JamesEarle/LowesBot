﻿namespace LowesBot.Models
{
    public class StoreInfo
    {
        public string Name { get; set; } = "Lowes Store #123";
        public string Address1 { get; set; } = "123 West Main Street";
        public string Address2 { get; set; } = "Denver, Colorado";
        public string Phone { get; set; } = "(303) 123-1234";
        public string Hours { get; set; } = "Today 10AM-8PM";
        public string Distance { get; set; } = "About 1 mile away";
        public string Image { get; set; } = "https://s3-media2.fl.yelpcdn.com/bphoto/hxoTdky_mk9PHZhUR5oy1A/168s.jpg";
        public string Map { get; set; } = "http://yuguangzhang.com/blog/wp-content/uploads/2013/04/screenshot.2.png";

        public string ManagerImage { get; set; } = "https://pbs.twimg.com/profile_images/3647943215/d7f12830b3c17a5a9e4afcc370e3a37e_400x400.jpeg";
        public string ManagerName { get; set; } = "Marcus Parsons";
        public string ManagerTitle { get; set; } = "Store Manager";
        public string ManagerPhone { get; set; } = "(303) 123-1234";
    }
}