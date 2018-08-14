using System;
using System.Collections.Generic;
using System.Text;

namespace Cyclist.logic.usernetwork.user
{
    class User
    {
        public struct Favorite
        {
            String Label;
            String Address;
        }

        public enum RideType
        {
            BIKE,
            ELCTRIC_BIKE,
            ELECTRIC_SCOOTER,
            SEGWAY
        };

        public String UserId { get; set; }
        public String Fname { get; set; }
        public String Lname { get; set; }
        public byte Age { get; set; }
        public RideType Ride { get; set; }
        public String Home;
        public String Work;
        public LinkedList<Favorite> Favorites = new LinkedList<Favorite>();
    }
}
