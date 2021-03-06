﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    public class ConversationReply : INotifyPropertyChanged
    {

        void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        public long ConversationId { get; set; }
        public String Body { get; set; }
        public String Author { get; set; }
        public DateTimeOffset SendingTime { get; set; }

        public DateTime LocalTime
        {
            get { return SendingTime.ToLocalTime().DateTime; }

        }

        bool _isRead;
        public bool IsRead
        {
            get
            {
                return _isRead;
            }
            set
            {
                if (_isRead != value)
                {
                    _isRead = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ConversationReplyStatus Status { get; set; }
    }
}
