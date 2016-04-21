﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Denna kod har genererats av ett verktyg och modifierats av William Book (Aka. Wildbook).
//     Körtidsversion:2.0.50727.8689
//
//     Ändringar i denna fil kan orsaka fel och kommer att förloras om
//     koden återgenereras.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;

//
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
//

namespace Kian.Core.MyAnimeList.Objects.API.Anime
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class anime
    {
        private animeEntry[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entry", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public animeEntry[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class animeEntry : INotifyPropertyChanged
    {
        private string idField;

        private string titleField;

        private string englishField;

        private string synonymsField;

        private string episodesField;

        private string scoreField;

        private string typeField;

        private string statusField;

        private string start_dateField;

        private string end_dateField;

        private string synopsisField;

        private string imageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                if (value != this.idField)
                {
                    this.idField = value;
                    RaisePropertyChanged("id");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                if (value != this.titleField)
                {
                    this.titleField = value;
                    RaisePropertyChanged("title");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string english
        {
            get
            {
                return this.englishField;
            }
            set
            {
                if (value != this.englishField)
                {
                    this.englishField = value;
                    RaisePropertyChanged("english");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string synonyms
        {
            get
            {
                return this.synonymsField;
            }
            set
            {
                if (value != this.synonymsField)
                {
                    this.synonymsField = value;
                    RaisePropertyChanged("synonyms");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string episodes
        {
            get
            {
                return this.episodesField;
            }
            set
            {
                if (value != this.episodesField)
                {
                    this.episodesField = value;
                    RaisePropertyChanged("episodes");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string score
        {
            get
            {
                return this.scoreField;
            }
            set
            {
                if (value != this.scoreField)
                {
                    this.scoreField = value;
                    RaisePropertyChanged("score");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                if (value != this.typeField)
                {
                    this.typeField = value;
                    RaisePropertyChanged("type");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                if (value != this.statusField)
                {
                    this.statusField = value;
                    RaisePropertyChanged("status");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string start_date
        {
            get
            {
                return this.start_dateField;
            }
            set
            {
                if (value != this.start_dateField)
                {
                    this.start_dateField = value;
                    RaisePropertyChanged("start_date");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string end_date
        {
            get
            {
                return this.end_dateField;
            }
            set
            {
                if (value != this.end_dateField)
                {
                    this.end_dateField = value;
                    RaisePropertyChanged("end_date");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string synopsis
        {
            get
            {
                return this.synopsisField;
            }
            set
            {
                if (value != this.synopsisField)
                {
                    this.synopsisField = value;
                    RaisePropertyChanged("synopsis");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string image
        {
            get
            {
                return this.imageField;
            }
            set
            {
                if (value != this.imageField)
                {
                    this.imageField = value;
                    RaisePropertyChanged("image");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}