using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace PhoneNumberCircleIDMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            // Give the path of the folder which contains the CSV files
            String folderPath = @"A:\Course Work\512- Visualization\Assignment\A3";
            String inputFileName = @"\SangeetSwara_TotalCalls_Date.csv";
            int timeperiod = 6;

            // Call a function to store the hashtable consisting of mapping of a prefix of a phone number and telecom circle. 
            Hashtable phoneCircleMapping = prefixCircleIdMapping(folderPath);
          
            // This function generate the total calls obtained from each telecom circle
            mobileGeographicMapping(phoneCircleMapping, folderPath, inputFileName, @"\GeoMapping_SangeetSwara_TotalCalls.csv");

            // This function generate the total calls obtained from each telecom circle for specified time period
            mobileDateGographicMapping(phoneCircleMapping, folderPath, inputFileName, @"\GeoMapping_SangeetSwara_Day_TotalCalls.csv", timeperiod);
        }

        /* This function takes Mobil_Circle.csv as input
         * It converts the mapping of prefixes of mobile numbers and circle ids to a hashtab;e
         **/
        static Hashtable prefixCircleIdMapping(String path)
        {
            Hashtable phoneCircleMapping = new Hashtable();
            try
            {
                int flag = 0;

                // Read the prefix and telecom circle from input csv file: Mobile_Circle.csv
                using (StreamReader sr = new StreamReader(path + @"\Mobile_Circle.csv"))
                {
                    // using count variable so that we can ignore the headers 
                    int count = 0;
                    String line;
                    String[] partsofline = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //split the line using comma operator
                        partsofline = line.Split(',');

                        // insert all records in the hashtable except the first row which has headers in the CSV
                        if (count != 0)
                        {
                            // store the prefix of mobile number
                            String prefix = partsofline[0];
                            // store the circleId
                            String circleId = partsofline[1];

                            // trim the prefix and circle id to get rid of leading and trailing spaces
                            prefix = prefix.Trim('"');
                            circleId = circleId.Trim('"');

                            // If circleId is empty for a prefix, then set it value as Not Defined
                            if (circleId.Equals(""))
                                circleId = "Not Defined";

                            /* Check whether the hastable has an entry for prefix or not
                             * if the hashtable does not contain an entry for the prefix, then add the circleId and prefix
                             * if the hashtable contains an entry already for a prefix, check whether the stored value is not defined? 
                             * if the stored value is not defined, then replace the value if the new value is other than not defined
                             * if the stored value is other than not defined, and if the new value is also other than not defined but different than old value, then replace the old value by new value
                             * */
                            bool isKeyPresent = phoneCircleMapping.ContainsKey(prefix);

                            if (!isKeyPresent)
                            {
                                phoneCircleMapping.Add(prefix, circleId);

                            }
                            else
                            {
                                flag++;
                                String circleIdStored = Convert.ToString(phoneCircleMapping[prefix]);

                                if (circleIdStored.Equals("Not Defined") && !circleId.Equals("Not Defined"))
                                {
                                    phoneCircleMapping.Remove(prefix);
                                    phoneCircleMapping[prefix] = circleId;
                                }
                                else if (!circleIdStored.Equals("Not Defined") && !circleId.Equals("Not Defined") && !circleIdStored.Equals(circleId))
                                {
                                    Console.WriteLine(prefix, circleId, circleIdStored);
                                }

                            }

                        }

                        count++;

                    }



                }

                // print the list on the console to check if the hashtable has correct entries or not. 
                foreach (string key in phoneCircleMapping.Keys)
                {
                    Console.WriteLine(String.Format("{0}: {1}", key, phoneCircleMapping[key]));
                }
                Console.WriteLine("Total Lines are:" + phoneCircleMapping.Count);
                Console.ReadKey();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return phoneCircleMapping;
        }

        /* This function takes mobile number prefix and circle mapping. It also needs the path of the folder containing CSV files
         * It also take the name of input file and name of output file as input
         * The functions output a CSV file consisting of total calls for each telecom circle in India 
         * */
        static void mobileGeographicMapping(Hashtable phoneCircleMapping, String folderPath, String inputFileName, String outputFileName)
        {

            try
            {
                // We are creating a hashtable consisting of circle ids as keys and frequency of calls as value
                Hashtable circleIDCallFrequency = new Hashtable();
                // we are also creating a list consisting of illformed numbers. Ill-formed numbers are number which has digits other than 10 or 12. 
                ArrayList unrecognizedNumbers = new ArrayList();


                using (StreamReader sr = new StreamReader(folderPath + inputFileName ))
                {
                    String line;
                    String[] partsofline = null;
                    int count = 0;
                    // Read each line of the input file one by one till the end of line is reached
                    while ((line = sr.ReadLine()) != null)
                    {
                        // this is so as to skip the headers of the columns
                        if (count == 0)
                        {
                            count++;
                            continue;
                        }

                        // split the line by comma delimiter to get phoneNumber from the line
                        partsofline = line.Split(',');
                        String phoneNumber = partsofline[0];
                        //String date = partsofline[1];

                        // trim the phone number to remove any extra spaces present
                        phoneNumber = phoneNumber.Trim('"');
                       
                        /* if phone number is not emoty and the number of digits in phone number is 10 or 12
                         * then extract the prefix, match the prefix with the prefix-circle mapping table
                         * if the match happens then add the value of circlid and set the count as one. 
                         * if circleid is already present, then increment the value by 1. 
                         * if the match is not present then add the value not defined and set the count as one
                         * if not defined category is already created then increment the value by 1
                         * 
                         * */
 
                        if (!phoneNumber.Equals("") && (phoneNumber.Length == 10 || phoneNumber.Length == 12))
                        {
                            String prefix = null;

                            if (phoneNumber.Length == 12)
                                prefix = phoneNumber.Substring(2, 4);
                            else
                                prefix = phoneNumber.Substring(0, 4);

                            bool isPrefixPresent = phoneCircleMapping.ContainsKey(prefix);

                            if (isPrefixPresent)
                            {
                                String circleId = Convert.ToString(phoneCircleMapping[prefix]);

                                bool isCirclIdPresent = circleIDCallFrequency.ContainsKey(circleId);

                                if (isCirclIdPresent)
                                {
                                    circleIDCallFrequency[circleId] = Convert.ToInt64(circleIDCallFrequency[circleId]) + 1;
                                }
                                else
                                    circleIDCallFrequency.Add(circleId, 1);
                            }
                            else
                            {
                                bool isNotDefinedKeyPresent = circleIDCallFrequency.ContainsKey("Not Defined");

                                if (isNotDefinedKeyPresent)
                                {
                                    circleIDCallFrequency["Not Defined"] = Convert.ToInt64(circleIDCallFrequency["Not Defined"]) + 1; ;
                                }
                                else
                                {
                                    circleIDCallFrequency.Add("Not Defined", 1);
                                }


                            }


                        }
                        /* If the number of digits in phone number is less than 10, then consider the number as ill-formed
                         * The number should also be added to the unrecognized number's list. 
                         * */
                        else
                            unrecognizedNumbers.Add(phoneNumber);
                    }

                }

                using (StreamWriter writer = new StreamWriter(folderPath + outputFileName, true))
                {
                    // write the output in a CSV file
                    writer.WriteLine("Circle ID, Frequency");

                    foreach (DictionaryEntry entry in circleIDCallFrequency)
                    {
                        writer.WriteLine(entry.Key + "," + entry.Value);
                    }

                    /*
                    writer.WriteLine("----------------------------------------------");

                    writer.WriteLine("Unrecognized Phone Numbers are listed below");

                    foreach (var entry in unrecognizedNumbers)
                    {
                        writer.WriteLine(entry.ToString());
                    }
                     * */
                }

            }
            // catch if any exceptions are caught!
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        /* This function creates a CSV file where total calls for a specific repeating time period 
         *  for each telecom circle is calculated. The function takes prefix-circleId mapping CSV, name of the destination folder, 
         *  name of the input file of data, name of the output CSV file and the time period duration
         *  For weekly segregation of data, input timeperiod as 6
         *  For daily segreation of data, input timeperiod as 1
         *  For breaking data for every 5th day, set timeperiod as 5
         * */
        static void mobileDateGographicMapping(Hashtable phoneCircleMapping, String folderPath, String inputFileName, 
            String outputFileName, int timePeriod)
        {
            try
            {
                // This hastable will contain values for each time period for each circle id
                Hashtable circleDateCallFrequency = new Hashtable();
                // This list will contain list of illformed numbers
                ArrayList unrecognizedNumbers = new ArrayList();


                using (StreamReader sr = new StreamReader(folderPath + inputFileName))
                {
                    String line;
                    String[] partsofline = null;
                    int count = 0;
                    DateTime startDate = new DateTime();
                    DateTime endDate = new DateTime();
                    int countTimePeriod = 0;
                    
                    // Read each line of the input file one by one
                    while ((line = sr.ReadLine()) != null)
                    {
                        // this is so as to skip the headers of the columns
                        if (count == 0)
                        {
                            count++;
                            continue;
                        }

                        // split the line using comma delimiter and store mobilenumber and date
                        partsofline = line.Split(',');
                        String phoneNumber = partsofline[0];
                        String datePart = partsofline[1];

                        // trim leading and trailing spaces from both mobilenumber and date
                        phoneNumber = phoneNumber.Trim('"');
                        datePart = datePart.Trim('"');

                        // conver the String date to DateTime date format
                        DateTime date = Convert.ToDateTime(datePart);

                        // set the endDate as current date + time period
                        if (count == 1)
                        {
                            count++;
                            startDate = date;
                            endDate = startDate.AddDays(timePeriod);
                        }

                        /* if enddate is less than the date read from the file, then increase enddate by adding timeperiod
                         * we should incrementally add timepriod to enddate untill enddate is greater than current date
                         * this is important if say current date is 4 weeks ahead of enddate
                         * countTimePeriod stores the units of timeperiod elapsed 
                         * */
                        while (DateTime.Compare(endDate, date) < 0)
                        {
                            countTimePeriod++;
                            endDate = endDate.AddDays(timePeriod);
                        }

                        /* If the phone number is wellformed, then prefix is computed. 
                         * If prefix is present, then value corresponding to the prefix is updated. 
                         * If the date falls into current unit of time, the value is incremented by 1
                         * If the date is ahead of the current unit of time, then the value of last unit of time 
                         * is copied as the value for current unit of time and then in incremented by 1
                         * 
                         * If prefix is not present, then prefix is inserted in the hashtable and 
                         * value for the current unit of time period is initialized as 1 and all units before that is initialized as 1
                         * */
                        if (!phoneNumber.Equals("") && (phoneNumber.Length == 10 || phoneNumber.Length == 12))
                        {
                            String prefix = null;

                            if (phoneNumber.Length == 12)
                                prefix = phoneNumber.Substring(2, 4);
                            else
                                prefix = phoneNumber.Substring(0, 4);

                            bool isPrefixPresent = phoneCircleMapping.ContainsKey(prefix);

                            if (isPrefixPresent)
                            {
                                String circleId = Convert.ToString(phoneCircleMapping[prefix]);

                                bool isCirclIdPresent = circleDateCallFrequency.ContainsKey(circleId);

                                if (isCirclIdPresent)
                                {
                                    ArrayList timePeriodCircleData = (ArrayList)circleDateCallFrequency[circleId];

                                    int numberOfWeeksInList = timePeriodCircleData.Count;
                                    int totalCallsLastWeek = Convert.ToInt32(timePeriodCircleData[numberOfWeeksInList - 1]);

                                    while (numberOfWeeksInList - 1 < countTimePeriod)
                                    {
                                        timePeriodCircleData.Add(totalCallsLastWeek);
                                        numberOfWeeksInList++;
                                    }

                                    timePeriodCircleData[countTimePeriod] = Convert.ToInt32(timePeriodCircleData[countTimePeriod]) + 1;

                                    circleDateCallFrequency[circleId] = timePeriodCircleData;
                                }
                                else
                                {

                                    ArrayList timePeriodCircleData = new ArrayList();
                                    for (int i = 0; i < countTimePeriod; i++)
                                    {
                                        timePeriodCircleData.Add(0);
                                    }
                                    timePeriodCircleData.Add(1);
                                    circleDateCallFrequency.Add(circleId, timePeriodCircleData);
                                }
                            }
                            else
                            {
                                bool isNotDefinedKeyPresent = circleDateCallFrequency.ContainsKey("Not Defined");

                                if (isNotDefinedKeyPresent)
                                {
                                    ArrayList timePeriodCircleData = (ArrayList)circleDateCallFrequency["Not Defined"];

                                    int numberOfWeeksInList = timePeriodCircleData.Count;
                                    int totalCallsLastWeek = Convert.ToInt32(timePeriodCircleData[numberOfWeeksInList - 1]);

                                    while (numberOfWeeksInList - 1 < countTimePeriod)
                                    {
                                        timePeriodCircleData.Add(totalCallsLastWeek);
                                        numberOfWeeksInList++;
                                    }

                                    timePeriodCircleData[countTimePeriod] = Convert.ToInt32(timePeriodCircleData[countTimePeriod]) + 1;
                                    circleDateCallFrequency["Not Defined"] = timePeriodCircleData;
                                }
                                else
                                {
                                    ArrayList timePeriodCircleData = new ArrayList();
                                    for (int i = 0; i < countTimePeriod; i++)
                                    {
                                        timePeriodCircleData.Add(0);
                                    }
                                    timePeriodCircleData.Add(1);
                                    circleDateCallFrequency.Add("Not Defined", timePeriodCircleData);
                                }


                            }


                        }
                        // add the numnber to the unrecognized number's list if it is illformed
                        else
                            unrecognizedNumbers.Add(phoneNumber);
                    }

                }

                using (StreamWriter writer = new StreamWriter(folderPath + outputFileName, true))
                {
                    // Store the Data
                    String excelHeader = "CircleID";

                    int maximum = 0;

                    // This is to know maximum unit of time period across all the circle Ids
                    foreach (DictionaryEntry entry in circleDateCallFrequency)
                    {
                        ArrayList timePeriodData = (ArrayList)entry.Value;

                        if (maximum < timePeriodData.Count)
                            maximum = timePeriodData.Count; 
                    }

                    // set the output value of time period in CSV file
                    String timePeriodValue = "Period";
                    if (timePeriod == 1)
                        timePeriodValue = "Day";
                    else if (timePeriod == 6)
                        timePeriodValue = "Week";

                    // Write the header for each column in the CSV file
                    for (int i = 0; i < maximum; i++)
                        excelHeader = excelHeader+ "," + timePeriodValue + (i+1);


                    writer.WriteLine(excelHeader);

                    // Write the data in CSV file
                    foreach (DictionaryEntry entry in circleDateCallFrequency)
                    {
                        ArrayList timePeriodData = (ArrayList)entry.Value;

                        String excelRow = Convert.ToString(entry.Key);

                        for (int i = 0; i < timePeriodData.Count; i++)
                        {
                            excelRow = excelRow + "," + timePeriodData[i];
                        }


                        writer.WriteLine(excelRow);
                    }

                    /*
                    writer.WriteLine("----------------------------------------------");

                    writer.WriteLine("Unrecognized Phone Numbers are listed below");

                    foreach (var entry in unrecognizedNumbers)
                    {
                        writer.WriteLine(entry.ToString());
                    }
                     **/
                }


            }
            // Catch any exception which might come
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}