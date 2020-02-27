using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asg2_xxy180008
{
    class FileIO
    {
        public static void saveToFile(String fileName, List<RebateForm> forms)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
            
            foreach (RebateForm rf in forms)
            {
                file.WriteLine(rf.ToString());
            }

            file.Close();
        }
        
        public static List<RebateForm> loadFromFile(String fileName)
        {
            List<RebateForm> forms = new List<RebateForm>();
            
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);

            String l; // Let l be the entire line when read the file line by line
            while ((l = file.ReadLine()) != null)
            {
                String[] fields = l.Split(',');

                RebateForm rb = new RebateForm();
                rb.FirstName = fields[0];
                rb.MName = fields[1];
                rb.LastName = fields[2];
                rb.Address1 = fields[3];
                rb.Address2 = fields[4];
                rb.City = fields[5];
                rb.State = fields[6];
                rb.Zip = fields[7];
                rb.Gender = fields[8][0];
                rb.Phone = fields[9];
                rb.Email = fields[10];
                if (fields[11][0].ToString().Equals("TRUE")) rb.Proof = true;
                else rb.Proof = false;
                rb.DateReceived = Convert.ToDateTime(fields[12]);

                forms.Add(rb); // Add to the list
         
            }

            file.Close();

            return forms;
        }
    }
}
