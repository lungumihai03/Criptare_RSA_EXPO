RSA Encryption via Exponentiation
=================================

Overview
--------

**RSA Encryption via Exponentiation** is a Windows Forms application developed in C# that implements the **RSA public-key cryptosystem** using arbitrary-precision arithmetic (BigInteger). It enables users to generate RSA keys, encrypt and decrypt text messages, and view all intermediate parameters (p, q, n, φ(n), e, d).

This is a **complete and functional** RSA implementation, featuring probabilistic prime generation (Miller-Rabin test), modular inverse computation, and block-based encryption/decryption.

* * *

Features
--------

* **RSA Key Generation**:
  * Generates large prime numbers p and q with configurable digit length (10–500 digits).
  * Automatically computes:
    * n = p × q
    * φ(n) = (p−1)(q−1)
    * e coprime with φ(n)
    * d = e⁻¹ mod φ(n)
* **Miller-Rabin Primality Test**: 15 rounds for high probabilistic confidence.
* **Block-Based Encryption**: Splits input into blocks smaller than n for secure processing.
* **UTF-8 Support**: Handles full Unicode text (including special characters).
* **Parameter Display**:
  * Dedicated buttons to show P, Q, N, Φ(N), E, D.
* **User-Friendly Interface**: Three text boxes for plaintext, ciphertext, and decrypted message.
* **Error Handling**: Clear messages for key generation, encryption, or decryption failures.

* * *

How It Works
------------

### 1. **Key Generation (Gen Key)**

1. Two large primes p and q are generated using the Miller-Rabin test.
2. Computes:
   * n = p × q (modulus)
   * φ(n) = (p−1)(q−1) (Euler's totient)
3. Selects e such that gcd(e, φ(n)) = 1
4. Computes d as the modular inverse of e modulo φ(n) → d × e ≡ 1 (mod φ(n))

> **Public Key**: (e, n) **Private Key**: (d, n)

* * *

### 2. **Encryption (Cript)**

* Input message is converted to UTF-8 bytes.
* Split into blocks where each block value < n.
* For each block m: c = m^e mod n
* Output: space-separated list of ciphertext blocks.

* * *

### 3. **Decryption (Decript)**

* Parse ciphertext blocks.
* For each block c: m = c^d mod n
* Reconstruct byte array and convert back to UTF-8 string.

* * *

Usage
-----

1. **Set Key Size**:
   * Use the numeric control (default: 100 digits) to set prime size.
   * Larger = more secure, slower.
2. **Generate Keys**:
   * Click **"Gen Key"**.
   * Wait for primes to be generated (may take time for large sizes).
3. **View Parameters**:
   * Click any of:
     * afiseaza P, Q, N, Fi(N), E, D
4. **Encrypt**:
   * Enter plaintext in **"Mesajul"**.
   * Click **"Cript"**.
   * Ciphertext appears in **"Mesajul Criptat"**.
5. **Decrypt**:
   * Ensure ciphertext is in **"Mesajul Criptat"**.
   * Click **"Decript"**.
   * Original message appears in **"Mesajul Decriptat"**.
