using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMN.Model
{
    public class ReportRecord
    {
        private int _employeeNumber;
        private DateTime _recordDate;
        private DateTime _inTime;
        private int _lateIn = 0;
        private int _earlyIn = 0;
        private DateTime _outTime;
        private int _earlyOut = 0;
        private int _lateOut = 0;
        private string _workTime;
        private string _overTime;
        private string _status = "";
        private DateTime _companyInTime;
        private DateTime _companyOutTime;

        public ReportRecord(int employeeNo, DateTime recordDate,DateTime inTime,DateTime outTime, DateTime companyInTime,DateTime companyOutTime)
        {
            this._employeeNumber = employeeNo;
            this._recordDate = recordDate;
            this._inTime = inTime;
            this._outTime = outTime;
            this._companyInTime = companyInTime;
            this._companyOutTime = companyOutTime;

            TimeSpan actualWorkedHours = outTime.Subtract(inTime);
            this._workTime = String.Format("{0}:{1}:{2}", actualWorkedHours.Hours,actualWorkedHours.Minutes,actualWorkedHours.Seconds);

            if(inTime > companyInTime)
            {
                this._lateIn = (int)Math.Round(inTime.Subtract(companyInTime).TotalMinutes);
            }
            else if (inTime < companyInTime)
            {
                this._earlyIn = (int)Math.Round(companyInTime.Subtract(inTime).TotalMinutes);
            }

            if (outTime > companyOutTime)
            {
                this._lateOut = (int)Math.Round(outTime.Subtract(companyOutTime).TotalMinutes);
            }
            else if (outTime < companyOutTime)
            {
                this._earlyOut = (int)Math.Round(companyOutTime.Subtract(outTime).TotalMinutes);
            }

            TimeSpan workHours = companyOutTime.Subtract(companyInTime);
            if (actualWorkedHours.TotalMinutes > workHours.TotalMinutes)
            {
                TimeSpan ovrtime = actualWorkedHours - workHours;
                this._overTime = String.Format("{0}:{1}:{2}", ovrtime.Hours, ovrtime.Minutes, ovrtime.Seconds);
            }

        }

        public DateTime CompanyOutTime
        {
            get { return _companyOutTime; }
            set { _companyOutTime = value; }
        }
        

        public DateTime CompanyInTime
        {
            get { return _companyInTime; }
            set { _companyInTime = value; }
        }

        public string OverTime
        {
            get { return _overTime; }
            set { _overTime = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        

        public string WorkTime
        {
            get { return _workTime; }
            set { _workTime = value; }
        }
        

        public int LateOut
        {
            get { return _lateOut; }
            set { _lateOut = value; }
        }
        

        public int EarlyOut
        {
            get { return _earlyOut; }
            set { _earlyOut = value; }
        }
        

        public DateTime OutTime
        {
            get { return _outTime; }
            set { _outTime = value; }
        }
        public string DisplayOutTime
        {
            get { return _outTime.ToString("hh:mm tt"); }
        }

        public int EarlyIn
        {
            get { return _earlyIn; }
            set { _earlyIn = value; }
        }
        

        public int LateIn
        {
            get { return _lateIn; }
            set { _lateIn = value; }
        }
        

        public DateTime InTime
        {
            get { return _inTime; }
            set { _inTime = value; }
        }
        
        public string DisplayInTime
        {
            get { return _inTime.ToString("hh:mm tt"); }
        }

        public DateTime RecordDate
        {
            get { return _recordDate; }
            set { _recordDate = value; }
        }
        public string DisplayRecordDate
        {
            get { return _recordDate.ToString("dd/MM/yyyy"); }
        }
       

        public int EmployeeNumber
        {
            get { return _employeeNumber; }
            set { _employeeNumber = value; }
        }
        
    }
}
