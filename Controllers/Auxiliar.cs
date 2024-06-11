namespace redeSocial_WebApplication.Controllers
{
    public static class Auxiliar
    {
        public static string fixedDigits(int num, int digits)
        {
            int currentNumLength = num.ToString().Length;
            string formatedNum = "";
            for (int i = 0; i + currentNumLength < digits; i++)
            {
                formatedNum += "0";
            }
            formatedNum += num.ToString();
            return formatedNum;
        }
    }
}
