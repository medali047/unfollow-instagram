using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // Open dialog to select the followers file
        string followersFilePath = OpenFile("Select the followers list file");
        if (string.IsNullOrEmpty(followersFilePath))
        {
            Console.WriteLine("No file selected for followers.");
            return;
        }

        // Open dialog to select the following file
        string followingFilePath = OpenFile("Select the following list file");
        if (string.IsNullOrEmpty(followingFilePath))
        {
            Console.WriteLine("No file selected for following.");
            return;
        }

        // Load followers and following lists from the selected text files
        List<string> followers = LoadUsernamesFromFile(followersFilePath);
        List<string> following = LoadUsernamesFromFile(followingFilePath);

        // Find users who don't follow you back
        List<string> notFollowingBack = following.Except(followers).ToList();

        // Output the results to the console
        Console.WriteLine("Users who don't follow you back:");
        foreach (var user in notFollowingBack)
        {
            Console.WriteLine(user);
        }

        // Save the results to a text file
        SaveResults(notFollowingBack);
    }

    static string OpenFile(string title)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Title = title;
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
        }

        return null;
    }

    static List<string> LoadUsernamesFromFile(string filePath)
    {
        // Read all lines from the file and return as a list
        return File.ReadAllLines(filePath).ToList();
    }

    static void SaveResults(List<string> notFollowingBack)
    {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Title = "Save users who don't follow you back";
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.FileName = "not_following_back.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(saveFileDialog.FileName, notFollowingBack);
                Console.WriteLine($"\nResults saved to: {saveFileDialog.FileName}");
            }
            else
            {
                Console.WriteLine("Save operation was canceled.");
            }
        }
    }
}
