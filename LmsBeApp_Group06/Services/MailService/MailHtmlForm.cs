using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Services
{
    public static class MailHtmlForm
    {
        public static string VetifyForm(string name, string urlHome, string urlVerify, string email)
        {
            return string.Format(@"
                <table style='margin: 0px; padding: 0px;width: 100%;text-align: center; padding: 1rem;' bgcolor='#fafafa'>
                    <tr style='height: 4px; background-color:aqua;'></tr>
                    <tr>
                        <td>
                            <h1 style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif; color: #9CA3AF;'>
                                TECHLMS</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table
                                style='width:640px;margin: 0 auto;border-collapse:separate;border-spacing:0;border-radius: 1rem; border: #F3F4F6 1px solid; padding: 1rem;'
                                bgcolor='#ffffff'>
                                <td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;text-align: center'>
                                    <h1 style='color: #333333;font-size: 18px;font-weight: 400; text-align: center;'>HELLO, {0}!
                                    </h1>
                                    <p style='color: #333333;text-align: center;'>Thank you for registering an account on {1}</p>
                                    <p style='color: #333333;text-align: center;'>If you did not perform this request, you can
                                        safely ignore this email.</p>
                                    <p style='color: #333333;text-align: center;'>To complete the registration, you need to verify
                                        your email by accessing the link below</p>
                                    <a href='{2}' style='text-align:center;text-decoration: none; padding: 0 1rem;'>Verify email</a>
                                </td>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p style='margin: 0;text-align: center;'>Everyone can contribute</p>
                            <p style='margin: 0;text-align: center;'>{3}</p>
                        </td>
                    </tr>
                </table>
                ", name, urlHome, urlVerify, email);
        }

        public static string InviteForm(User user, Class _class, string urlAccept, string email)
        {
            return string.Format(@"
                <table style='margin: 0px; padding: 0px;width: 100%;text-align: center; padding: 1rem;' bgcolor='#fafafa'>
                    <tr style='height: 4px; background-color:aqua;'></tr>
                    <tr>
                        <td>
                            <h1 style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif; color: #9CA3AF;'>
                                TECHLMS</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table
                                style='width:640px;margin: 0 auto;border-collapse:separate;border-spacing:0;border-radius: 1rem; border: #F3F4F6 1px solid; padding: 1rem;'
                                bgcolor='#ffffff'>
                                <td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;text-align: center'>
                                    <h1 style='color: #333333;font-size: 18px;font-weight: 400; text-align: center;'>HELLO, {0}!
                                    </h1>
                                    <p style='color: #333333;text-align: center;'>You are invited to <span style='font-weight: 700;'>{1}</span></p>
                                    <p style='color: #333333;text-align: center;'>To accept, please click the link below</p>
                                    <a href='{2}' style='text-align:center;text-decoration: none; padding: 0 1rem;'>Join to class</a>
                                </td>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p style='margin: 0;text-align: center;'>Everyone can contribute</p>
                            <p style='margin: 0;text-align: center;'>{3}</p>
                        </td>
                    </tr>
                </table>
                ", user.Username, _class.Name, urlAccept, email);
        }
    }
}