using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBravo
{
    public partial class Form1 : Form
    {

        int pageNumber = 1;
        int rowOfPage = 50;

        public Form1()
        {

            InitializeComponent();

            int totalRecord = 0;
            using (TestBravoEntities db = new TestBravoEntities())
            {
                totalRecord = db.tbl_Customer.Count();
            };

            
            lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, totalRecord / rowOfPage);

        }
        
        private async Task LoadDataCustomer(int pageNumber, int rowOfPage)
        {
           
            string connStr = "Data Source=THANG-PC\\SQLEXPRESS; Initial Catalog=TestBravo; User ID=sa;Password=sa123";
            string query = @"DECLARE @PageNumber AS INT
                            DECLARE @RowsOfPage AS INT
                            SET @PageNumber="+pageNumber+"SET @RowsOfPage="+rowOfPage+"SELECT Id, CustomerCode as [Mã], Fullname as [Tên], Address as [Địa chỉ] FROM [tbl_Customer]ORDER BY Id OFFSET (@PageNumber-1)*@RowsOfPage ROWS FETCH NEXT @RowsOfPage ROWS ONLY";


            SqlConnection conn = new SqlConnection(connStr);
            
               conn.Open();

                SqlCommand sqlCommand = new SqlCommand(query, conn);

                DataTable data = new DataTable();
                
                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    data.Load(reader);
                
                }
                
                conn.Close();
            gvCustomer.DataSource = data;
        }


        private async void btnNext_Click(object sender, EventArgs e)
        {
            int totalRecord = 0;
            using (TestBravoEntities db = new TestBravoEntities())
            {
                totalRecord = db.tbl_Customer.Count();
            };
            if (pageNumber < totalRecord / rowOfPage)
            {
                
                pageNumber++;
                await LoadDataCustomer(pageNumber, rowOfPage);
                lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, totalRecord / rowOfPage);

            }
        }

        private async void btnPre_Click(object sender, EventArgs e)
        {
            int totalRecord = 0;
            using (TestBravoEntities db = new TestBravoEntities())
            {
                totalRecord = db.tbl_Customer.Count();
            };
            if (pageNumber - 1 > 0)
            {
                pageNumber--;
                await LoadDataCustomer(pageNumber, rowOfPage);
                lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, totalRecord / rowOfPage);
            }
            
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
             await LoadDataCustomer(pageNumber, rowOfPage);
        }
    }
}
