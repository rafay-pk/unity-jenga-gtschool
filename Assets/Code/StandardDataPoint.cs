using System;
using System.Collections.Generic;

namespace Code
{
	[Serializable] public class StandardDataListParser
	{
		public List<StandardDataPointParser> dataPoints;
	}
	[Serializable] public class StandardDataPointParser
	{
		public int id;
		public string subject;
		public string grade;
		public int mastery;
		public string domainid;
		public string domain;
		public string cluster;
		public string standardid;
		public string standarddescription;
	}
	[Serializable] public class StandardDataPoint
	{
		public int id;
		public string subject;
		public Grade grade;
		public Mastery mastery;
		public string domainID;
		public string domain;
		public string cluster;
		public string standardID;
		public string standardDescription;
		
		public StandardDataPoint(StandardDataPointParser parser)
		{
			id = parser.id;
			subject = parser.subject;
			grade = ParseGrade(parser.grade);
			mastery = ParseMastery(parser.mastery);
			domainID = parser.domainid;
			domain = parser.domain;
			cluster = parser.cluster;
			standardID = parser.standardid;
			standardDescription = parser.standarddescription;
		}

		public static Grade ParseGrade(string str)
		{
			return str switch
			{
				"6th Grade" => Grade.Sixth,
				"7th Grade" => Grade.Seventh,
				"8th Grade" => Grade.Eighth,
				"Algebra I" => Grade.Algebra1,
				_ => Grade.Null
			};
		}
		public static Mastery ParseMastery(int i)
		{
			return i switch
			{
				0 => Mastery.NeedToLearn,
				1 => Mastery.Learned,
				2 => Mastery.Mastered,
				_ => Mastery.Null
			};
		}
	}
	public enum Grade
	{
		Null = 0, Sixth = 6, Seventh = 7, Eighth = 8, Algebra1 = 9
	}
	public enum Mastery
	{
		Null, NeedToLearn, Learned, Mastered
	}
}