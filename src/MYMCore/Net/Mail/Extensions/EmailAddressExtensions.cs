using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MYMCore.Net.Mail {
    public static class EmailAddressExtensions {
        public static bool TryParse(this string s, out MailAddress mailAddress) {
            mailAddress = null;
            try {

                mailAddress = new MailAddress(s);

                return true;

            }
            catch (FormatException) {

                return false;
            }
        }

        public static void BatchEmailAddressValidation(this IEnumerable<string> emailAddress, out IEnumerable<MailAddress> validMailAddress, out IEnumerable<string> invalidMailAddress) {

            validMailAddress = null;
            invalidMailAddress = null;

            if (emailAddress == null || emailAddress.Count() == 0) {
                return;
            }

            var valid = new List<MailAddress>();
            var invalid = new List<string>();
            var pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            foreach (var item in emailAddress) {
                MailAddress mail;
                if (item.TryParse(out mail) && Regex.IsMatch(item.Trim(), pattern)) {
                    valid.Add(mail);
                }
                else {
                    invalid.Add(item);
                }
            }

            validMailAddress = valid.Any() ? valid : null;

            invalidMailAddress = invalid.Any() ? invalid : null;
        }

        public static void BatchEmailAddressValidation(this string s, out IEnumerable<MailAddress> validMailAddress, out IEnumerable<string> invalidMailAddress) {
            if (string.IsNullOrWhiteSpace(s)) {
                throw new ArgumentNullException(nameof(s));
            }

            s.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .BatchEmailAddressValidation(out validMailAddress, out invalidMailAddress);
        }
    }
}
