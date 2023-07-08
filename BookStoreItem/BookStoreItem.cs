using System.Globalization;

[assembly: CLSCompliant(true)]

namespace BookStoreItem
{
    /// <summary>
    /// Represents an item in a book store.
    /// </summary>
    public class BookStoreItem
    {
        private readonly string authorName;
        private readonly string? isni;
        private readonly bool hasIsni;
        private decimal price;
        private string currency;
        private int amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        public BookStoreItem(string authorName, string title, string publisher, string isbn)
            : this(authorName, null, title, publisher, isbn, null, string.Empty, 0, "USD", 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="isni"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="isni">A book author's ISNI.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        public BookStoreItem(string authorName, string isni, string title, string publisher, string isbn)
            : this(authorName, isni, title, publisher, isbn, null, string.Empty, 0, "USD", 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>, <paramref name="published"/>, <paramref name="bookBinding"/>, <paramref name="price"/>, <paramref name="currency"/> and <paramref name="amount"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        /// <param name="published">A book publishing date.</param>
        /// <param name="bookBinding">A book binding type.</param>
        /// <param name="price">An amount of money that a book costs.</param>
        /// <param name="currency">A price currency.</param>
        /// <param name="amount">An amount of books in the store's stock.</param>
        public BookStoreItem(string authorName, string title, string publisher, string isbn, DateTime? published, string bookBinding, decimal price, string currency, int amount)
            : this(authorName, null, title, publisher, isbn, published, bookBinding, price, currency, amount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="isni"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>, <paramref name="published"/>, <paramref name="bookBinding"/>, <paramref name="price"/>, <paramref name="currency"/> and <paramref name="amount"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="isni">A book author's ISNI.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        /// <param name="published">A book publishing date.</param>
        /// <param name="bookBinding">A book binding type.</param>
        /// <param name="price">An amount of money that a book costs.</param>
        /// <param name="currency">A price currency.</param>
        /// <param name="amount">An amount of books in the store's stock.</param>
        public BookStoreItem(string authorName, string isni, string title, string publisher, string isbn, DateTime? published, string bookBinding, decimal price, string currency, int amount)
        {
            this.authorName = authorName;
            this.isni = isni;
            this.hasIsni = ValidateIsni(isni);
            this.Title = title;
            this.Isbn = isbn;
            this.Publisher = publisher;
            this.Published = published;
            this.BookBinding = bookBinding;
            this.price = price;
            this.currency = currency;
            this.amount = amount;

            if (!this.HasIsni && isni != null)
            {
                throw new ArgumentException("Invalid ISNI code", nameof(isni));
            }

            if (!ValidateIsbnFormat(isbn) || !ValidateIsbnChecksum(isbn))
            {
                throw new ArgumentException("Invalid ISBN code", nameof(isbn));
            }

            if (string.IsNullOrWhiteSpace(authorName))
            {
                throw new ArgumentException("Invalid author name", nameof(authorName));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Invalid title name", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(publisher))
            {
                throw new ArgumentException("Invalid publisher name", nameof(publisher));
            }

            ThrowExceptionIfCurrencyIsNotValid(currency);
        }

        /// <summary>
        /// Gets a book author's name.
        /// </summary>
        public string AuthorName => this.authorName;

        /// <summary>
        /// Gets an International Standard Name Identifier (ISNI) that uniquely identifies a book author.
        /// </summary>
        public string? Isni => this.isni;

        /// <summary>
        /// Gets a value indicating whether an author has an International Standard Name Identifier (ISNI).
        /// </summary>
        public bool HasIsni => this.hasIsni;

        /// <summary>
        /// Gets a book title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a book publisher.
        /// </summary>
        public string Publisher { get; private set; }

        /// <summary>
        /// Gets a book International Standard Book Number (ISBN).
        /// </summary>
        public string Isbn { get; private set; }

        /// <summary>
        /// Gets or sets a book publishing date.
        /// </summary>
        public DateTime? Published { get; set; }

        /// <summary>
        /// Gets or sets a book binding type.
        /// </summary>
        public string BookBinding { get; set; }

        /// <summary>
        /// Gets or sets an amount of money that a book costs.
        /// </summary>
        public decimal Price
        {
            get
            {
                return this.price;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Price));
                }

                this.price = value;
            }
        }

        /// <summary>
        /// Gets or sets a price currency.
        /// </summary>
        public string Currency
        {
            get
            {
                return this.currency;
            }

            set
            {
                ThrowExceptionIfCurrencyIsNotValid(value);
                this.currency = value;
            }
        }

        /// <summary>
        /// Gets or sets an amount of books in the store's stock.
        /// </summary>
        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Amount));
                }

                this.amount = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Uri"/> to the contributor's page at the isni.org website.
        /// </summary>
        /// <returns>A <see cref="Uri"/> to the contributor's page at the isni.org website.</returns>
        public Uri GetIsniUri()
        {
            if (string.IsNullOrWhiteSpace(this.isni))
            {
                throw new InvalidOperationException("ISNI hadn't set up");
            }

            return new Uri($"https://isni.org/isni/{this.isni}");
        }

        /// <summary>
        /// Gets an <see cref="Uri"/> to the publication page on the isbnsearch.org website.
        /// </summary>
        /// <returns>an <see cref="Uri"/> to the publication page on the isbnsearch.org website.</returns>
        public Uri GetIsbnSearchUri()
        {
            return new Uri($"https://isbnsearch.org/isbn/{this.Isbn}");
        }

        /// <summary>
        /// Returns the string that represents a current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string representationPrice = this.price.ToString("N", CultureInfo.InvariantCulture);
            if (representationPrice.Contains(','))
            {
                representationPrice = $"\"{representationPrice} {this.Currency}\"";
            }
            else
            {
                representationPrice = $"{representationPrice} {this.Currency}";
            }

            if (string.IsNullOrWhiteSpace(this.isni))
            {
                return $"{this.Title}, {this.AuthorName}, ISNI IS NOT SET, {representationPrice}, {this.Amount}";
            }
            else
            {
                return $"{this.Title}, {this.AuthorName}, {this.Isni}, {representationPrice}, {this.Amount}";
            }
        }

        private static bool ValidateIsni(string? isni)
        {
            if (string.IsNullOrEmpty(isni) || isni.Length != 16)
            {
                return false;
            }

            foreach (char c in isni)
            {
                if (!char.IsDigit(c) && c != 'X')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateIsbnFormat(string isbn)
        {
            if (isbn.Length != 10)
            {
                return false;
            }

            foreach (char c in isbn)
            {
                if (!char.IsDigit(c) && c != 'X')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateIsbnChecksum(string isbn)
        {
            int checksum = 0;

            for (int i = 0; i < isbn.Length; i++)
            {
                int value;
                if (isbn[i] == 'X')
                {
                    value = 10;
                }
                else
                {
                    if (!int.TryParse(isbn[i].ToString(), out value))
                    {
                        return false;
                    }
                }

                checksum += (10 - i) * value;
            }

            return checksum % 11 == 0;
        }

        private static void ThrowExceptionIfCurrencyIsNotValid(string currency)
        {
            if (currency.Length != 3)
            {
                throw new ArgumentException("Invalid currency", nameof(currency));
            }

            foreach (char c in currency)
            {
                if (!char.IsLetter(c))
                {
                    throw new ArgumentException("Invalid currency", nameof(currency));
                }
            }
        }
    }
}
