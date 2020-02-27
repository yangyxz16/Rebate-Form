using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asg2_xxy180008
{
    class RebateForm
    {
        private String firstName;
        private String mName;
        private String lastName;
        private String address1;
        private String address2;
        private String city;
        private String state;
        private String zip;
        private Char gender;
        private String phone;
        private String email;
        private Boolean proof;
        private DateTime dateReceived;

        // Below three variables are not visiable to the user
        private DateTime startTime;
        private DateTime endTime;
        private int backcount;

        
        public string FirstName { get => firstName; set => firstName = value; }
        public String MName { get => mName; set => mName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Address1 { get => address1; set => address1 = value; }
        public string Address2 { get => address2; set => address2 = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }
        public string Zip { get => zip; set => zip = value; }
        public char Gender { get => gender; set => gender = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public bool Proof { get => proof; set => proof = value; }
        public DateTime DateReceived { get => dateReceived; set => dateReceived = value; }
        
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
        public int Backcount { get => backcount; set => backcount = value; }

        public RebateForm(){}

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FirstName).Append(",")
                .Append(MName).Append(",")
                .Append(LastName).Append(",")
                .Append(Address1).Append(",")
                .Append(Address2).Append(",")
                .Append(City).Append(",")
                .Append(State).Append(",")
                .Append(Zip).Append(",")
                .Append(Gender).Append(",")
                .Append(Phone).Append(",")
                .Append(Email).Append(",")
                .Append(Proof.ToString()).Append(",")
                .Append(DateReceived.ToShortDateString()).Append(",")
                .Append(startTime.ToLongTimeString()).Append(",")
                .Append(endTime.ToLongTimeString()).Append(",")
                .Append(backcount);

            return sb.ToString();
        }
    }
}
