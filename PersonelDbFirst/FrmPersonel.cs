using PersonelDbFirst.Context;
using PersonelDbFirst.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonelDbFirst
{
    public partial class FrmPersonel : Form
    {
        public FrmPersonel()
        {
            InitializeComponent();
        }
        PersonelContext db = new PersonelContext();
        Personel secPer;
        private void FrmPersonel_Load(object sender, EventArgs e)
        {
            Doldur();

            //dataGridView1.DataSource = db.Set<Personel>().ToList();

            cmDoldur();
            cmUnvan();
        }

        private void Doldur()
        {
            dataGridView1.DataSource = db.Set<Personel>().Select(x => new PersonelDTO
            {
                Id = x.PersonelId,
                AdSoy = x.Ad + " " + x.Soyad,
                Yoneticisi = x.Personel2.Ad + " " + x.Personel2.Soyad,
                Unvan=x.Unvan.UnvanAd


            }).ToList();
        }

        private void cmUnvan()
        {
            cbUnvan.DataSource = db.Set<Unvan>().Select(x => new
            {
                x.UnvanId,
                x.UnvanAd


            }).ToList();
            cbUnvan.DisplayMember = "UnvanAd";
            cbUnvan.ValueMember = "UnvanId";
        }

        private void cmDoldur()
        {
            cbYonetici.DataSource = db.Set<Personel>().Where(x => x.Personel1.Count>0).Select(x => new
            {
                x.PersonelId,
                Adsoy=x.Ad+x.Soyad,
                

            }).ToList();
            cbYonetici.DisplayMember = "Adsoy";
            cbYonetici.ValueMember = "PersonelId";
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int secId = (int)dataGridView1.CurrentRow.Cells[0].Value;
            secPer = db.Set<Personel>().Find(secId);
            txAd.Text = secPer.Ad;
            txSoyad.Text = secPer.Soyad;
            cbYonetici.SelectedValue = secPer.YoneticiId;
            cbUnvan.SelectedValue = secPer.UnvanId;
            ICollection<Personel> plist = secPer.Personel1;
            lsEleman.Items.Clear();
            foreach (var item in plist)
            {
                lsEleman.Items.Add(item);
            }


        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Personel p = new Personel();
            p.Ad = txAd.Text;
            p.Soyad = txSoyad.Text;
            p.UnvanId=Convert.ToInt32(cbUnvan.SelectedValue);
            p.YoneticiId =Convert.ToInt32(cbYonetici.SelectedValue);
            db.Set<Personel>().Add(p);
            db.SaveChanges();
            Doldur();
            




        }
    }
}
