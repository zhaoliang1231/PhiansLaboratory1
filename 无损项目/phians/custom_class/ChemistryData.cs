using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public class ChemistryData
    {

        public string id { get; set; }

        public string group_id { get; set; }
        public string Standard_id { get; set; }
        public string elements_name { get; set; }

        public string elementsName
        {
            get
            {

                if (elements_name != "")
                {
                    return elements_name;
                }
                else
                {
                    return "";
                }
            }

            set
            {

            }

        }



        public string test1 { get; set; }

        private string _requireData1 = "";

        public string requireData1
        {
            get
            {
                if (test1 == "True")
                {
                    if (data3 == "True")
                    {
                        return "Information";
                    }
                    else
                    {
                        if (data1 != "" && data2 != "")
                            return data1 + "-" + data2;
                        if (data1 != "" && data2 == "")
                            return data1 + "min";
                        if (data1 == "" && data2 != "")
                            return data2 + "max";
                        else
                            return _requireData1;
                    }
                }
                else
                {
                    return _requireData1;
                }
            }
            set
            {
                _requireData1 = value;
            }
        }

        public string data1 { get; set; }

        public string data2 { get; set; }

        public string data3 { get; set; }

        public string type2 { get; set; }


     

        public string test2 { get; set; }

        private string _requireData2 = "";

        public string requireData2
        {
            get
            {
                if (test2 == "True")
                {
                    if (data6 == "True")
                    {
                        return "Information";
                    }
                    else
                    {
                        if (data4 != "" && data5 != "")
                            return data4 + "-" + data5;
                        if (data4 != "" && data5 == "")
                            return data4 + "min";
                        if (data4 == "" && data5 != "")
                            return data5 + "max";
                        else
                            return _requireData2;
                    }
                }
                else
                {
                    return _requireData2;
                }
            }
            set
            {
                _requireData2 = value;
            }
        }

        public string data4 { get; set; }

        public string data5 { get; set; }

        public string data6 { get; set; }

        public string type3 { get; set; }


    

        public string test3 { get; set; }

        private string _requireData3 = "";

        public string requireData3
        {
            get
            {
                if (test3 == "True")
                {
                    if (data9 == "True")
                    {
                        return "Information";
                    }
                    else
                    {
                        if (data7 != "" && data8 != "")
                            return data7 + "-" + data8;
                        if (data7 != "" && data8 == "")
                            return data7 + "min";
                        if (data1 == "" && data8 != "")
                            return data8 + "max";
                        else
                            return _requireData3;
                    }
                }
                else
                {
                    return _requireData3;
                }
            }
            set
            {
                _requireData3 = value;
            }
        }

        public string data7 { get; set; }

        public string data8 { get; set; }

        public string data9 { get; set; }

        public string type4 { get; set; }


        private string _valueType4;


        public string test4 { get; set; }

        private string _requireData4 = "";

        public string requireData4
        {
            get
            {
                if (test4 == "True")
                {
                    if (data12 == "True")
                    {
                        return "Information";
                    }
                    else
                    {
                        if (data10 != "" && data11 != "")
                            return data10 + "-" + data11;
                        if (data10 != "" && data11 == "")
                            return data10 + "min";
                        if (data10 == "" && data11 != "")
                            return data11 + "max";
                        else
                            return _requireData4;
                    }
                }
                else
                {
                    return _requireData4;
                }
            }
            set
            {
                _requireData4 = value;
            }
        }

        public string data10 { get; set; }

        public string data11 { get; set; }

        public string data12 { get; set; }


    }
}