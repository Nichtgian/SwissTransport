using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SwissTransport;

namespace SwissTransportView
{
    class ModelView : INotifyPropertyChanged
    {
        /*swiss transport api*/
        private Transport transport = new Transport();

        /*list of best matching stations from input*/
        private Hints hints = new Hints();

        public Hints Hints
        {
            get { return hints; }
            set { hints = value; OnPropertyChanged("Hints"); }
        }

        /*selected station names*/
        private Selected selected = new Selected();

        public Selected Selected
        {
            get { return selected; }
            set { selected = value; OnPropertyChanged("Selected"); }
        }

        /*connections between from and to station*/
        private Connections connections = new Connections();

        public Connections Connections
        {
            get { return connections; }
            set { connections = value; OnPropertyChanged("Connections"); }
        }

        /*connections between from and to station*/
        private Board board = new Board();

        public Board Board
        {
            get { return board; }
            set { board = value; OnPropertyChanged("Board"); }
        }

        /*interface INotifyPropertyChanged*/
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /*get station hints from input with api*/
        public void getStationHints(string input)
        {
            Hints.Stations = transport.GetStations(input).StationList;
            OnPropertyChanged("Hints");
        }

        /*selected station from dropdown*/
        public void selectStation(bool from, string station)
        {
            if (from)
            {
                Selected.From = station;
            }
            else
            {
                Selected.To = station;
            }
            OnPropertyChanged("Selected");
        }

        /*get a list of connections between input stations*/
        public void getConnections()
        {
            string errors = "";

            if (Selected.From == null || Selected.From == "")
            {
                errors += "From Station is Empty!\n";
            }
            if (Selected.To == null || Selected.To == "")
            {
                errors += "To Station is Empty!\n";
            }

            if (errors == "")
            {
                List<Station> from = transport.GetStations(Selected.From).StationList;
                if (from == null || from.Count <= 0)
                {
                    errors += "From station not found\n";
                }
                else
                {
                    Selected.From = from[0].Name;
                    OnPropertyChanged("Selected");
                }

                List<Station> to = transport.GetStations(Selected.To).StationList;
                if (to == null || to.Count <= 0)
                {
                    errors += "To station not found\n";
                }
                else
                {
                    Selected.To = to[0].Name;
                    OnPropertyChanged("Selected");
                }

                if (errors == "" && Selected.From == Selected.To)
                {
                    errors += "From and To Stations are the same!\n";
                }
            }

            string date = "";
            try
            {
                DateTime selectedDate = DateTime.Parse((string)Selected.Date);
                date = selectedDate.ToString("yyyy-MM-dd");
            }
            catch(Exception)
            {
                errors += "Entered Date(" + Selected.Date + ") is invalid!\n";
            }

            string time = "";
            try
            {
                DateTime selectedTime = DateTime.Parse((string)Selected.Time);
                time = selectedTime.ToString("HH:mm");
            }
            catch (Exception)
            {
                errors += "Entered Time(" + Selected.Time + ") is invalid!\n";
            }

            if (errors == "")
            {
                try
                {
                    Connections.List = transport.GetConnections(Selected.From, Selected.To, date, time).ConnectionList;
                    OnPropertyChanged("Connections");
                }
                catch (Exception)
                {
                    MessageBox.Show("No connection!");
                }
            }
            else
            {
                MessageBox.Show(errors);
            }
        }

        /*selected a connection to get more details*/
        public void getConnectionDetails(Connection connection)
        {
            MessageBox.Show("From: " + connection.From.Station.Name + "\nDeparting at " + DateTime.Parse(connection.From.Departure) + " on rail " + connection.From.Platform + "\n\n" +
                "Travel Duration: " + connection.Duration + "\n\n" +
                "To: " + connection.To.Station.Name + "\nArrive at " + DateTime.Parse(connection.To.Arrival) + " on rail " + connection.To.Platform);
        }

        /*get a list of connections going from one station*/
        public void getBoard()
        {
            List<Station> stations = null;
            string errors = "";

            if (Selected.From == null || Selected.From == "")
            {
                errors += "Station is Empty!\n";
            }

            if (errors == "")
            {
                try
                {
                    stations = transport.GetStations(Selected.From).StationList;
                }
                catch (Exception)
                {
                    errors += "Station not found\n";
                }

                if (errors == "" && stations != null && stations.Count > 0)
                {
                    Station station = transport.GetStations(Selected.From).StationList[0];
                    Selected.From = station.Name;
                    OnPropertyChanged("Selected");

                    try
                    {
                        Board.List = transport.GetStationBoard(station.Name, station.Id).Entries;
                        OnPropertyChanged("Board");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error at getting Board occured: " + e.ToString());
                    }
                }
                else
                {
                    MessageBox.Show(errors);
                }
            }
        }

        /*set date yyyy-MM-dd of depart*/
        public void setDate(string date)
        {
            Selected.Date = date;
            OnPropertyChanged("Selected"); 
        }

        /*set time HH:mm of depart*/
        public void setTime(string time)
        {
            Selected.Time = time;
            OnPropertyChanged("Selected");
        }

        /*opens google maps in browser at station location*/
        public void getMap()
        {
            List<Station> stations = null;
            string errors = "";

            if (Selected.From == null || Selected.From == "")
            {
                errors += "Station is Empty!\n";
            }

            if (errors == "")
            {
                try
                {
                    stations = transport.GetStations(Selected.From).StationList;
                }
                catch (Exception)
                {
                    errors += "Station not found\n";
                }

                if (errors == "" && stations != null && stations.Count > 0)
                {
                    Station station = transport.GetStations(Selected.From).StationList[0];
                    Selected.From = station.Name;
                    OnPropertyChanged("Selected");

                    try
                    {
                        System.Diagnostics.Process.Start("https://www.google.com/maps/?q=" + station.Coordinate.XCoordinate + "," + station.Coordinate.YCoordinate);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error opening maps in browser: " + e.ToString());
                    }
                }
                else
                {
                    MessageBox.Show(errors);
                }
            }
        }
    }
}
