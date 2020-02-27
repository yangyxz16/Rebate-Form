using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asg2_xxy180008
{
    public partial class Form1 : Form
    {
        List<RebateForm> forms; // List to store rebate form
        String[] preModified; // List to store currently modifying record's first name, last name and phone number
        private const String fileName = "CS6326Asg2.txt"; // The name of the text file

        DateTime startTime; // Variable to store the start time
        int backCount = 0; // Variable to count back space

        bool modifySave = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            forms = FileIO.loadFromFile(fileName); // Load data from file to store into the forms list 
            preModified = new string[3];

            // Display all the data onto the ListView
            foreach (RebateForm rf in forms)
            {
                addToListView(rf);
            }

            // Default: save, modify and delete features are disabled
            btModify.Enabled = false;
            btDelete.Enabled = false;

            

            // Set date picker default date which is today
            datePicker.Value = DateTime.Today;
            
            // Restrict datepicker from selecting future date
            datePicker.MaxDate = DateTime.Today;
            tbFName.Focus();
        }




        /// <summary>
        /// Count the times that user presses back space
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
            {
                backCount += 1;
            }
        }

        /// <summary>
        /// When the start to enter date to first name text box, record the start time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFName_Enter(object sender, EventArgs e)
        {
            startTime = DateTime.Now;

            lbWarning.Visible = false;
        }



        private void btSave_Click_1(object sender, EventArgs e)
        {
            RebateForm rb = collectDataFromInput();

            if (checkInput()) // Check each input
            {
                if (dupForm(rb)) // No duplicate record
                {
                    if (modifySave) // Modifying
                    {
                        // Update the Rebate Form List
                        forms[forms.IndexOf((RebateForm)listView.SelectedItems[0].Tag)] = rb;
                        // Update the list view
                        listView.SelectedItems[0].SubItems[0].Text = rb.FirstName;
                        listView.SelectedItems[0].SubItems[1].Text = rb.LastName;
                        listView.SelectedItems[0].SubItems[2].Text = rb.Phone;

                        modifySave = false;
                        disableTB();
                        btSave.Enabled = false;

                        // Write to File.
                        FileIO.saveToFile(fileName, forms);

                        lbWarning.Text = "DATA SUCCESSFULLY SAVED";
                        lbWarning.Visible = true;
                        clearDataFields();
                    }
                    else // Inputting new
                    {
                        // Only record StartTime, EndTime and Backcount fields when creating new, remain unchanged during modification
                        rb.StartTime = startTime;
                        rb.EndTime = DateTime.Now;
                        rb.Backcount = backCount;

                        // Add to the forms list.
                        forms.Add(rb);
                        // Display on the ListView
                        addToListView(rb);

                        // Write to File.
                        FileIO.saveToFile(fileName, forms);

                        lbWarning.Text = "RECORD SUCCESSFULLY ADDED";
                        lbWarning.Visible = true;
                        clearDataFields();
                    }
                }
            }
        }

        private void btModify_Click(object sender, EventArgs e)
        {
            modifySave = true;
            enableTB(); // Enable input fields

            // Record the current moditying record's first name, last name, phone number
            var selected = (RebateForm)listView.SelectedItems[0].Tag;
            preModified[0] = selected.FirstName;
            preModified[1] = selected.LastName;
            preModified[2] = selected.Phone;

            btSave.Enabled = true;

            // During Modifying, both modify and delete buttons disables unless user presses reset button
            btModify.Enabled = false;
            btDelete.Enabled = false;
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            // Delete from the RebateForm list
            forms.Remove((RebateForm)listView.SelectedItems[0].Tag);
            // Delete from the list view
            listView.Items.Remove(listView.SelectedItems[0]);
            // Update the file
            FileIO.saveToFile(fileName, forms);

            lbWarning.Text = "RECORD DELETED";
            lbWarning.Visible = true;
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            clearDataFields();

            btModify.Enabled = true;
            btDelete.Enabled = true;
            btSave.Enabled = true;
            enableTB();

            lbWarning.ResetText();

            backCount = 0;

            modifySave = false;
        }

        
        
        /// <summary>
        /// When the selected item is changed of the list view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 0) // When user selects row
            {
                var selected = (RebateForm)listView.SelectedItems[0].Tag; // Create a variable to save the selected RebateForm

                if (selected != null)
                {
                    // Display the selected RebateForm info
                    displaySelected(selected);
                    // Diable all the Text Box
                    disableTB();
                    btModify.Enabled = true;
                    btDelete.Enabled = true;
                }
            }

            else // When no row is selected
            {
                // Claer the data fields
                clearDataFields();

                enableTB();

                // Do not allow user to modify or delete when no item is selected
                btModify.Enabled = false;
                btDelete.Enabled = false;
            }

        }



        /// <summary>
        /// Make sure only one of the male/female checkbox is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbM_CheckedChanged(object sender, EventArgs e)
        {
            if (cbF.Enabled) cbF.Enabled = false;
            else cbF.Enabled = true;
        }

        /// <summary>
        /// Make sure only one of the male/female checkbox is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbF_CheckedChanged(object sender, EventArgs e)
        {
            if (cbM.Enabled) cbM.Enabled = false;
            else cbM.Enabled = true;
        }



        /// <summary>
        /// Create RebateForm object and save all the text boxes data
        /// </summary>
        /// <returns>RebateForm object</returns>
        private RebateForm collectDataFromInput()
        {
            // Save all the date to become an RebateForm object
            RebateForm rb = new RebateForm();
            
            rb.FirstName = tbFName.Text;
            rb.MName = tbMName.Text;
            rb.LastName = tbLName.Text;
            rb.Address1 = tbAddress1.Text;
            rb.Address2 = tbAddress2.Text;
            rb.City = tbCity.Text;
            rb.State = tbState.Text;
            rb.Zip = tbZip.Text;
            if (cbM.Checked) rb.Gender = 'M';
            if (cbF.Checked) rb.Gender = 'F';
            rb.Phone = tbPhone.Text;
            rb.Email = tbEmail.Text;
            if (cbProof.Checked) rb.Proof = true;
            else rb.Proof = false;
            rb.DateReceived = datePicker.Value;

            return rb;
        }

        /// <summary>
        /// Add a rebate form to the list view
        /// </summary>
        /// <param name="rf"></param>
        private void addToListView(RebateForm rf)
        {
            var row = new String[] { rf.FirstName, rf.LastName, rf.Phone };
            var lvi = new ListViewItem(row);
            // Add the object to the Tag property so that it can be used to be selected
            lvi.Tag = rf;
            // Add the item to the list view control
            listView.Items.Add(lvi);
        }

        /// <summary>
        /// When a row of the list view is selected, display all of its detabe form info 
        /// </summary>
        /// <param name="rf"></param>
        private void displaySelected(RebateForm rf)
        {
            tbFName.Text = rf.FirstName;
            tbMName.Text = rf.MName;
            tbLName.Text = rf.LastName;
            tbAddress1.Text = rf.Address1;
            tbAddress2.Text = rf.Address2;
            tbCity.Text = rf.City;
            tbState.Text = rf.State;
            tbZip.Text = rf.Zip;
            if (rf.Gender == 'M') { cbM.Checked = true; }
            else if (rf.Gender == 'F') { cbF.Checked = true; }

            tbPhone.Text = rf.Phone;
            tbEmail.Text = rf.Email;
            if (rf.Proof) { cbProof.Checked = true; }
            datePicker.Value = rf.DateReceived;
        }

        /// <summary>
        /// Clear all the text boxes texts
        /// </summary>
        private void clearDataFields()
        {
            tbFName.Text = "";
            tbMName.Text = "";
            tbLName.Text = "";
            tbAddress1.Text = "";
            tbAddress2.Text = "";
            tbCity.Text = "";
            tbState.Text = "";
            tbZip.Text = "";
            cbM.Checked = false;
            cbF.Checked = false;
            tbPhone.Text = "";
            tbEmail.Text = "";
            cbProof.Checked = false;
            datePicker.Value = DateTime.Today;

        }



        /// <summary>
        /// Disable all the text boxes
        /// </summary> 
        private void disableTB()
        {
            tbFName.Enabled = false;
            tbMName.Enabled = false;
            tbLName.Enabled = false;
            tbAddress1.Enabled = false;
            tbAddress2.Enabled = false;
            tbCity.Enabled = false;
            tbState.Enabled = false;
            tbZip.Enabled = false;
            cbM.Enabled = false;
            cbF.Enabled = false;
            tbPhone.Enabled = false;
            tbEmail.Enabled = false;
            datePicker.Enabled = false;
        }

        /// <summary>
        /// Enable all the text boxes
        /// </summary>
        private void enableTB()
        {
            tbFName.Enabled = true;
            tbMName.Enabled = true;
            tbLName.Enabled = true;
            tbAddress1.Enabled = true;
            tbAddress2.Enabled = true;
            tbCity.Enabled = true;
            tbState.Enabled = true;
            tbZip.Enabled = true;
            cbM.Enabled = true;
            cbF.Enabled = true;
            tbPhone.Enabled = true;
            tbEmail.Enabled = true;
            datePicker.Enabled = true;
        }
   

        
        /// <summary>
        /// Check if the object with same first name, last name and phone number exists
        /// </summary>
        /// <param name="rf"></param>
        /// <returns> True if existed </returns>
        private bool dupForm(RebateForm rf)
        {
            foreach (RebateForm form in forms)
            {
                
                if (modifySave && rf.FirstName == preModified[0] && rf.LastName == preModified[1] && rf.Phone == preModified[2])
                {
                    return true;
                }
                
                // Duplicate
                if (rf.FirstName == form.FirstName && rf.LastName == form.LastName && rf.Phone == form.Phone)
                {
                    lbWarning.Text = "INVALID: RECORD EXISTS!";
                    lbWarning.Visible = true;
                    return false;
                }
            }
            return true;    
        }

        /// <summary>
        /// Check each text box's input
        /// </summary>
        /// <returns> true when all of the input are valid </returns>
        private bool checkInput()
        {
            if (inputValidating(tbFName, "FIRST NAME")
                && inputValidating(tbLName, "LAST NAME")
                && inputValidating(tbAddress1, "ADDRESS")
                && inputValidating(tbCity, "CITY")
                && inputValidating(tbState, "STATE")
                && inputValidating(tbZip, "ZIP CODE")
                && genderValidating()
                && inputValidating(tbPhone, "PHONE NUMBER")
                && inputValidating(tbEmail, "EMAIL")
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Method to Check for each text box input
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="s"></param>
        private bool inputValidating(TextBox tb, String s)
        {
            if (string.IsNullOrEmpty(tb.Text)) // The textbox is empty
            {
                displayInputValidatedMsg(s);
                return false;
            }
            else if (tb == tbPhone && phoneValidated(tb.Text)) // Check if phone number input is validates
            {
                return false;
            }
            else if (tb == tbEmail && emailValidated(tb.Text)) // Check if email input validates
            {
                return false;
            }
            else
            {
                lbWarning.Visible = false;
                return true;
            }
        }

        /// <summary>
        /// Method to check gender check boxes
        /// </summary>
        /// <returns> false if the gender has not been selected </returns>
        private bool genderValidating()
        {
            if (!cbM.Checked && !cbF.Checked)
            {
                lbWarning.Text = "WARNING: MUST SELECT A GENDER";
                lbWarning.Visible = true;
                return false;
            }
            else return true;
        }

        /// <summary>
        /// Method to check phone text box input
        /// </summary>
        /// <param name="s"></param>
        /// <returns> true if phone number is invalid </returns>
        private bool phoneValidated(String s)
        {
            foreach (char c in s)
            {
                if ((c  < '0') || (c > '9')) // Check each character see if it is a number
                {
                    lbWarning.Text = "INVALID PHONE NUMBER";
                    lbWarning.Visible = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method to check email text box input
        /// </summary>
        /// <param name="s"></param>
        /// <returns> true if email address is invalid </returns>
        private bool emailValidated(String s)
        {
            System.Text.RegularExpressions.Regex reEmail = new System.Text.RegularExpressions.Regex(@"^*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"); // Regular Expression of email format

            if (!reEmail.IsMatch(s))
            {
                lbWarning.Text = "INVALID EMAIL ADDRESS";
                lbWarning.Visible = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Display the warning msg
        /// </summary>
        /// <param name="s"></param>
        private void displayInputValidatedMsg(String s)
        {
            lbWarning.Text = s + " CANNOT BE EMPTY";
            lbWarning.Visible = true;
        }

        
    }
}
