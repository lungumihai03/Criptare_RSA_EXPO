using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Forms;

namespace Criptare_RSA_EXPO
{
    public partial class Form1 : Form
    {
        BigInteger p, q, n, phi, e, d;

        public Form1()
        {
            InitializeComponent();
        }

        // ===================== GENERARE CHEI =====================
        private void btnGenerate_Click(object sender, EventArgs ev)
        {
            try
            {
                int digits = (int)numericUpDown1.Value;
                if (digits < 10) digits = 10;
                if (digits > 500) digits = 500;

                p = GenereazaPrim(digits);
                q = GenereazaPrim(digits);
                n = p * q;
                phi = (p - 1) * (q - 1);
                e = GenereazaE(phi);
                d = InverseMod(e, phi);

               

                MessageBox.Show($"Chei RSA ({digits} cifre) generate cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        // ===================== GENERARE NUMĂR PRIM =====================
        private BigInteger GenereazaPrim(int digits)
        {
            Random rnd = new Random();
            BigInteger numar;

            do
            {
                string s = "1";
                for (int i = 1; i < digits - 1; i++)
                    s += rnd.Next(0, 10).ToString();
                s += rnd.Next(1, 9).ToString();
                numar = BigInteger.Parse(s);
            } while (!MillerRabin(numar, 15));

            return numar;
        }

        // ===================== TEST MILLER–RABIN =====================
        private bool MillerRabin(BigInteger n, int k)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0) return false;

            BigInteger d = n - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            Random rnd = new Random();
            for (int i = 0; i < k; i++)
            {
                BigInteger a = RandomBigInteger(2, n - 2, rnd);
                BigInteger x = BigInteger.ModPow(a, d, n);
                if (x == 1 || x == n - 1)
                    continue;

                bool cont = false;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == n - 1)
                    {
                        cont = true;
                        break;
                    }
                }
                if (!cont) return false;
            }
            return true;
        }

        private BigInteger RandomBigInteger(BigInteger min, BigInteger max, Random rnd)
        {
            byte[] bytes = max.ToByteArray();
            BigInteger r;
            do
            {
                rnd.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F;
                r = new BigInteger(bytes);
            } while (r < min || r >= max);
            return r;
        }

        // ===================== GENERARE E =====================
        private BigInteger GenereazaE(BigInteger phi)
        {
            Random rnd = new Random();
            BigInteger e;
            do
            {
                e = RandomBigInteger(3, phi - 1, rnd);
            } while (BigInteger.GreatestCommonDivisor(e, phi) != 1);
            return e;
        }

        // ===================== INVERSE MODULAR =====================
        private BigInteger InverseMod(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m, y = 0, x = 1;
            if (m == 1) return 0;

            while (a > 1)
            {
                BigInteger q = a / m;
                BigInteger t = m;
                m = a % m;
                a = t;
                t = y;
                y = x - q * y;
                x = t;
            }

            if (x < 0) x += m0;
            return x;
        }

        // ===================== CRIPTARE =====================
        private void btnEncrypt_Click(object sender, EventArgs ev)
        {
            try
            {
                if (e == 0 || n == 0)
                {
                    MessageBox.Show("Generează mai întâi cheile!");
                    return;
                }

                byte[] data = Encoding.UTF8.GetBytes(txtPlain.Text);
                int blockSize = (n.ToString().Length / 3) - 1;
                List<string> blocks = new List<string>();

                for (int i = 0; i < data.Length; i += blockSize)
                {
                    int len = Math.Min(blockSize, data.Length - i);
                    byte[] block = new byte[len];
                    Array.Copy(data, i, block, 0, len);
                    BigInteger m = new BigInteger(block);
                    if (m < 0) m = -m;
                    BigInteger c = BigInteger.ModPow(m, e, n);
                    blocks.Add(c.ToString());
                }

                txtEncrypted.Text = string.Join(" ", blocks);
                MessageBox.Show("Mesaj criptat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la criptare: " + ex.Message);
            }
        }

        // ===================== DECRIPTARE =====================
        private void btnDecrypt_Click(object sender, EventArgs ev)
        {
            try
            {
                if (d == 0 || n == 0)
                {
                    MessageBox.Show("Generează mai întâi cheile!");
                    return;
                }

                string[] parts = txtEncrypted.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<byte> bytes = new List<byte>();

                foreach (string part in parts)
                {
                    BigInteger c = BigInteger.Parse(part);
                    BigInteger m = BigInteger.ModPow(c, d, n);
                    byte[] block = m.ToByteArray();
                    bytes.AddRange(block);
                }

                txtDecrypted.Text = Encoding.UTF8.GetString(bytes.ToArray());
                MessageBox.Show("Mesaj decriptat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la decriptare: " + ex.Message);
            }
        }

        // ===================== AFIȘARE CHEI =====================
        private void btnP_Click(object sender, EventArgs e)
        {
            MessageBox.Show("P = " + p);
        }

        private void btnQ_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Q = " + q);
        }

        private void btnN_Click(object sender, EventArgs e)
        {
            MessageBox.Show("N = " + n);
        }

        private void btnPhi_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Φ(N) = " + phi);
        }

        private void btnE_Click(object sender, EventArgs ev)
        {
            MessageBox.Show("E = " + e);
        }

        private void btnD_Click(object sender, EventArgs e)
        {
            MessageBox.Show("D = " + d);
        }
    }
}
