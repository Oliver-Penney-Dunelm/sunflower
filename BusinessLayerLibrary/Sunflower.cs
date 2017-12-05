using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerLibrary
{
    public class BigParameter
    {
        public string ParameterName { get; set; }
        public string ParameterType { get; set; }
        public string ParameterStringValue { get; set; }
        public int ParameterIntValue { get; set; }
        public decimal ParameterDecimalValue { get; set; }
        public DateTime ParameterDateTimeValue { get; set; }
    }

    public class Season
    {
        [Key]
        public int SeasonID { get; set; }
        [DisplayName("Season")]
        public string SeasonDesc { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FirstLaunchDate { get; set; }
        public int SeasonActive { get; set; }
        public decimal VAT { get; set; }
        public decimal GBPUSD { get; set; }
        public decimal GBPEUR { get; set; }
    }
    public class Attribute
    {
        [Key]
        public int AttributeID { get; set; }
        [DisplayName("Attribute")]
        public string AttributeDesc { get; set; }
        public int Seasonal { get; set; }
        public int AttributeOrder { get; set; }
        public string SapName { get; set; }
        public string PlmName { get; set; }
        public int DataTypeID { get; set; }
        public int FutureSeasonCascade { get; set; }
        public int Calculated { get; set; }
    }
    public class Team
    {
        [Key]
        public int TeamID { get; set; }
        [DisplayName("Team")]
        public string TeamDesc { get; set; }
        public string TeamEmail { get; set; }
    }
    //public class TeamUser
    //{
    //    [Key]
    //    public string ID
    //    {
    //        get { return string.Format("{0}{1}{2}", NetworkID, "_", TeamID); }
    //    }
    //    public string NetworkID { get; set; }
    //    public int TeamID { get; set; }
    //}
    public class AttributeValue
    {
        [Key]
        public string ID
        {
            get { return string.Format("{0}{1}{2}{3}{4}", SFID, "_", AttributeID, "_", SeasonID); }
        }
        public int SFID { get; set; }
        public int AttributeID { get; set; }
        public string AttributeDescription { get; set; }
        public int SeasonID { get; set; }
        public int AttributeOrder { get; set; }
        public string AttributeValueEntry { get; set; }
    }

    public class AttributeValuePivot
    {
        [Key]
        public string ID
        {
            get { return string.Format("{0}{1}{2}", SFID, "_", SeasonID); }
        }
        public int SFID { get; set; }
        [DisplayName("Desc")]
        public string SunflowerDesc { get; set; }
        public string SapArticleID { get; set; }
        public string MasterDescription { get; set; }
        public int SeasonID { get; set; }
        public int MerchCatID { get; set; }
        public string MerchCatDesc { get; set; }
        public string DeptName { get; set; }
        public string CatDesc { get; set; }
        public string VendorID { get; set; }
        public string VendorDesc { get; set; }
        public int ProposedPreArticleGradeID { get; set; }
        [DisplayName("Proposed Grade")]
        public string ProposedPreArticleGradeDesc { get; set; }
        public int ConfirmedPreArticleGradeID { get; set; }
        [DisplayName("Confirmed Grade")]
        public string ConfirmedPreArticleGradeDesc { get; set; }
        public string Att01 { get; set; }
        public string Att02 { get; set; }
        public string Att03 { get; set; }
        public string Att04 { get; set; }
        public string Att05 { get; set; }
        public string Att06 { get; set; }
        public string Att07 { get; set; }
        public string Att08 { get; set; }
        public string Att09 { get; set; }
        public string Att10 { get; set; }
        public string Att11 { get; set; }
        public string Att12 { get; set; }
        public string Att13 { get; set; }
        public string Att14 { get; set; }
        public string Att15 { get; set; }
        public string Att16 { get; set; }
        public string Att17 { get; set; }
        public string Att18 { get; set; }
    }

    public class TeamAttributePermission
    {
        [Key]
        public string ID
        {
            get { return string.Format("{0}{1}{2}", TeamID, "_", AttributeID); }
        }
        public int TeamID { get; set; }
        public int AttributeID { get; set; }
        public string AttributeDescription { get; set; }
        public int ReadPermission { get; set; }
        public int WritePermission { get; set; }
        public int Notify { get; set; }
        public int ViewChange { get; set; }
        public int ParkForQuarantine { get; set; }
    }

    public class PivotView
    {
        [Key]
        public int ViewID { get; set; }
        [DisplayName("View")]
        public string ViewDesc { get; set; }
    }

    public class User
    {
        [Key]
        public string NetworkID { get; set; }
        public int TeamID { get; set; }
    }

    public class ViewAttributePivot
    {
        [Key]
        public string ID
        {
            get { return string.Format("{0}{1}{2}", ViewID, "_", AttributeID); }
        }
        public int ViewID { get; set; }
        public int AttributeID { get; set; }
        public string AttributeDescription { get; set; }
        public int PivotOrder { get; set; }

    }
    public class PreArticleSeasonal
    {
        [Key]
        public string ID
        {
            get { return string.Format("{0}{1}{2}", SFID, "_", SeasonID); }
        }
        public int SFID { get; set; }
        public string SunflowerDesc { get; set; }
        public string PlmDescription { get; set; }
        public string PlmID { get; set; }
        public string SapArticleID { get; set; }
        public string SapDescription { get; set; }
        public int SeasonID { get; set; }
        public int ProposedPreArticleGradeID { get; set; }
        public int ConfirmedPreArticleGradeID { get; set; }
        public int MerchCatID { get; set; }
        public string VendorID { get; set; }
    }

    public class Grade
    {
        [Key]
        public int GradeID { get; set; }
        public string GradeDescription { get; set; }
        public int GradeOrder { get; set; }
        public int PreArticleGrade { get; set; }
        public int WholeGrade { get; set; }
        public int SubCatGrade { get; set; }
    }
    public class MerchCat
    {
        [Key]
        public int MerchCatID { get; set; }
        public int DeptID { get; set; }
        public int CatID { get; set; }
        public string MerchCatDesc { get; set; }
        public int MerchCatActive { get; set; }
    }
    public class Vendor
    {
        [Key]
        public string VendorID { get; set; }
        public string VendorDesc { get; set; }
    }
    public class Space
    {
        [Key]
        public int SpaceID { get; set; }
        public string SpaceDesc { get; set; }
        public int SpaceOrder { get; set; }
    }
    public class MeType
    {
        [Key]
        public int MeTypeID { get; set; }
        public string MeTypeDesc { get; set; }
    }
    public class SpaceAllocation
    {
        [Key]
        public string ID
        {
            get { return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", SeasonID, "_", SpaceID, "_", CatID, "_", SpaceGradeID, "_", FixtureOrdinal, "_", OrdinalSequence); }
        }
        public int AutoID { get; set; }
        public int SeasonID { get; set; }
        public int SpaceID { get; set; }
        public int CatID { get; set; }
        public int SpaceGradeID { get; set; }
        public int FixtureOrdinal { get; set; }
        public string FixtureName { get; set; }
        [DisplayName("LocationID")]
        public int OrdinalSequence { get; set; }
        public int SFID { get; set; }
        public string SapDescription { get; set; }
        public string ProposedGrade { get; set; }
        public string ConfirmedGrade { get; set; }
        public int Fill { get; set; }
        public int DisplayQuantity { get; set; }
        public int MeTypeID { get; set; }
        public string MeTypeDesc { get; set; }
        public int PosSequence { get; set; }
        public int Facings { get; set; }
        public int PosOccurances { get; set; }
    }
    public class Dept
    {
        [Key]
        public int DeptID { get; set; }
        [DisplayName("Dept")]
        public string DeptName { get; set; }
        [DisplayName("Dept Order")]
        public int DeptOrder { get; set; }
        public int DeptActive { get; set; }
    }
    public class Cat
    {
        [Key]
        public int CatID { get; set; }
        [DisplayName("Cat")]
        public string CatDesc { get; set; }
        public int CatActive { get; set; }
        public int CatOrder { get; set; }
        public int Concept { get; set; }
    }
    public class AvailableSpace
    {
        [Key]
        public string ID
        {
            get { return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", SeasonID, "_", SpaceID, "_", CatID, "_", SpaceGradeID, "_", FixtureOrdinal); }
        }
        public int SeasonID { get; set; }
        public int SpaceID { get; set; }
        [DisplayName("Cat")]
        public int CatID { get; set; }
        public int SpaceGradeID { get; set; }
        [DisplayName("Grade")]
        public string SpaceGradeDescription { get; set; }
        public int FixtureOrdinal { get; set; }
        public string FixtureName { get; set; }
    }

    public class SFDataType
    {
        [Key]
        public int DataTypeID { get; set; }
        public string DataTypeDescription { get; set; }
    }

}
