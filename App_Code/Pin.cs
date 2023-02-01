using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;

public class Pin
{
    private static int DEFAULT_MIN_PASSWORD_LENGTH = 4;
    private static int DEFAULT_MAX_PASSWORD_LENGTH = 6;
    private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
    private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
    private static string PASSWORD_CHARS_NUMERIC = "23456789";
    private static string PASSWORD_CHARS_SPECIAL = "trevqpmxrskbc";// "*$-+?_&=!%{}/";

    public static string Generate()
    {
        return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                        DEFAULT_MAX_PASSWORD_LENGTH);
    }

    public static string Generate(int length)
    {
        return Generate(length, length);
    }

    public static string Generate(int minLength, int maxLength)
    {
        if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
            return null;
        char[][] charGroups = new char[][] 
        {
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
        };
        int[] charsLeftInGroup = new int[charGroups.Length];

        for (int i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;

        int[] leftGroupsOrder = new int[charGroups.Length];

        for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;
        byte[] randomBytes = new byte[4];

        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        rng.GetBytes(randomBytes);
        int seed = (randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3];
        Random random = new Random(seed);
        char[] password = null;

        // Allocate appropriate memory for the password.
        if (minLength < maxLength)
            password = new char[random.Next(minLength, maxLength + 1)];
        else
            password = new char[minLength];

        // Index of the next character to be added to password.
        int nextCharIdx;

        // Index of the next character group to be processed.
        int nextGroupIdx;

        // Index which will be used to track not processed character groups.
        int nextLeftGroupsOrderIdx;

        // Index of the last non-processed character in a group.
        int lastCharIdx;

        // Index of the last non-processed group.
        int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        // Generate password characters one at a time.
        for (int i = 0; i < password.Length; i++)
        {
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0,
                                                     lastLeftGroupsOrderIdx);
            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;
            if (lastCharIdx == 0)
                nextCharIdx = 0;
            else
                nextCharIdx = random.Next(0, lastCharIdx + 1);
            password[i] = charGroups[nextGroupIdx][nextCharIdx];

            if (lastCharIdx == 0)
                charsLeftInGroup[nextGroupIdx] =
                                          charGroups[nextGroupIdx].Length;
            // There are more unprocessed characters left.
            else
            {
                // Swap processed character with the last unprocessed character
                // so that we don't pick it until we process all characters in
                // this group.
                if (lastCharIdx != nextCharIdx)
                {
                    char temp = charGroups[nextGroupIdx][lastCharIdx];
                    charGroups[nextGroupIdx][lastCharIdx] =
                                charGroups[nextGroupIdx][nextCharIdx];
                    charGroups[nextGroupIdx][nextCharIdx] = temp;
                }
                // Decrement the number of unprocessed characters in
                // this group.
                charsLeftInGroup[nextGroupIdx]--;
            }

            // If we processed the last group, start all over.
            if (lastLeftGroupsOrderIdx == 0)
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            // There are more unprocessed groups left.
            else
            {
                // Swap processed group with the last unprocessed group
                // so that we don't pick it until we process all groups.
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                    leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                leftGroupsOrder[nextLeftGroupsOrderIdx];
                    leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                }
                // Decrement the number of unprocessed groups.
                lastLeftGroupsOrderIdx--;
            }
        }

        // Convert password characters into a string and return the result.
        return new string(password);
    }

}




