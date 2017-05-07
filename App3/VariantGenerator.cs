using System;

namespace generator
{
    public enum Mounth
    {
        February = 2,
        September = 9
    }
    public class Varianter
    {
        private static int[] InformaticPart1 = { 13, 12, 13, 11 };
        private static int CountVariants(int numberLabwork)
        {
            return InformaticPart1[numberLabwork - 1];
        }
        private static void ValidateData(int year, int numberInList, int numberLabWork)
        {
            if (numberLabWork == 0)
                throw new ArgumentException("ЛР должна быть выбрана");
            if (year > DateTime.Now.Year)
            {
                throw new ArgumentException("Вы из будущего?");
            }
            if (year < 2012)
            {
                throw new ArgumentException("Год не корректен");
            }
            if (DateTime.Now.Day == 1 && DateTime.Now.Month == 1)
            {
                throw new ArgumentException("В Новый Год надо отдыхать, а не варианты генерировать :)");
            }
            if (numberInList < 1 || numberInList > 100)
            {
                throw new ArgumentException("Крайне подозрительный номер в списке");
            }
        }
        public static int GetVariantForInformatic(Mounth mounth, int year, int numberInList, int numberLabwork)
        {
            ValidateData(year, numberInList, numberLabwork);
            return (((((year%100) * 10 + (int)mounth) * 100 + numberInList) * 10 + numberLabwork) * 10 + numberInList) % CountVariants(numberLabwork) + 1;
        }
        public static int GetVariantForOther(string lastnamem)
        {
            return Math.Abs(lastnamem.GetHashCode() % 10) + 1;
        }

    }
}