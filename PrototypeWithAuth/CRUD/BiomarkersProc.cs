using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class BiomarkersProc : ApplicationDbContextProc<Test>
    {
        public BiomarkersProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                this.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> RunScriptsAsync()
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var testValues = _context.TestValues;
                foreach (var tv in testValues)
                {
                    _context.Remove(tv);
                }
                _context.SaveChanges();
                var testheaders = _context.TestHeaders;
                foreach (var th in testheaders)
                {
                    _context.Remove(th);
                }
                _context.SaveChanges();
                var testgroups = _context.TestGroups;
                foreach (var tg in testgroups)
                {
                    _context.Remove(tg);
                }
                _context.SaveChanges();
                var testoutergroups = _context.TestOuterGroups;
                foreach (var tog in testoutergroups)
                {
                    _context.Remove(tog);
                }
                _context.SaveChanges();
                var experimentTests = _context.Experiments.SelectMany(e => e.ExperimentTests);
                foreach (var et in experimentTests)
                {
                    _context.Remove(et);
                }
                _context.SaveChanges();
                var tests = _context.Tests;
                foreach (var t in tests)
                {
                    _context.Remove(t);
                }
                _context.SaveChanges();
                var experimentEntries = _context.ExperimentEntries;
                foreach (var ee in experimentEntries)
                {
                    _context.Remove(ee);
                }
                _context.SaveChanges();
                var sites = _context.Sites;
                foreach (var s in sites)
                {
                    _context.Remove(s);
                }
                _context.SaveChanges();
                var particpants = _context.Participants;
                foreach (var p in particpants)
                {
                    _context.Remove(p);
                }
                _context.SaveChanges();
                var experiments = _context.Experiments;
                foreach (var e in experiments)
                {
                    _context.Remove(e);
                }

                await _1InsertExeriments();
                await _2InsertSites();
                await _3Add02TestsBloodPressure();
                await _4InsertECGTest();
                await _5InsertAnthropometryTest();
                await _6InsertFlexibilityTest();
                await _7InsertBalanceTest();
                await _8InsertMuscleStrengthTest();
                await _9InsertCPETandSpirometryTest();
                await _10InsertDexaTest();
                await _11InsertImmunochemistryTest();
                await _12InsertBloodChemicalsTest();
                await _13InsertBloodCountTest();
                await _14InsertUltrasoundTest();
                await _15InsertAgeReaderTest();
                await _16InsertProcedureDocTest();

                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }


        private async Task _1InsertExeriments()
        {
            DateTime startDate;
            DateTime.TryParseExact("20210106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            DateTime endDate;
            DateTime.TryParseExact("20240106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            Experiment HighFrequency = new Experiment()
            {
                Description = "High Frequency",
                ExperimentCode = "ex1",
                NumberOfParticipants = 40,
                MinimumAge = 30,
                MaximumAge = 80,
                StartDateTime = startDate,
                EndDateTime = endDate
            };
            _context.Add(HighFrequency);
            DateTime.TryParseExact("20210106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            DateTime.TryParseExact("20240106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            Experiment LowFrequency = new Experiment()
            {
                Description = "Low Frequency",
                ExperimentCode = "ex2",
                NumberOfParticipants = 120,
                MinimumAge = 20,
                MaximumAge = 80,
                StartDateTime = startDate,
                EndDateTime = endDate
            };
            _context.Add(LowFrequency);
            _context.SaveChanges();
        }


        private async Task _2InsertSites()
        {
            Site Centarix = new Site()
            {
                Name = "Centarix Biotech",
                Line1Address = "Hamarpe 3",
                City = "Har Hotzvim",
                Country = "Jerusalem",
                PrimaryContactID = _context.Users.Where(u => u.Email == "rachelstrauss@centarix.com").FirstOrDefault().Id,
                PhoneNumber = "077-2634302"
            };
            _context.Add(Centarix);
            Site O2 = new Site()
            {
                Name = "O2",
                Line1Address = "",
                City = "Har Hazofim",
                Country = "Jerusalem",
                PrimaryContactID = _context.Users.Where(u => u.Email == "rachelstrauss@centarix.com").FirstOrDefault().Id,
                PhoneNumber = "055-9876543"
            };
            _context.Add(O2);
            _context.SaveChanges();
        }


        private async Task _3Add02TestsBloodPressure()
        {
            Test BloodPressure = new Test()
            {
                Name = "Blood Pressure",
                SiteID = _context.Sites.Where(s => s.Name == "O2").FirstOrDefault().SiteID
            };
            _context.Add(BloodPressure);
            _context.SaveChanges();
            ExperimentTest et1 = new ExperimentTest()
            {
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault(),
                TestID = _context.Tests.Where(t => t.Name == "Blood Pressure").Select(t => t.TestID).FirstOrDefault()
            };
            ExperimentTest et2 = new ExperimentTest()
            {
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault(),
                TestID = _context.Tests.Where(t => t.Name == "Blood Pressure").Select(t => t.TestID).FirstOrDefault()
            };
            _context.Add(et1);
            _context.Add(et2);
            _context.SaveChanges();
            TestOuterGroup bptog1 = new TestOuterGroup()
            {
                IsNone = true,
                TestID = _context.Tests.Where(t => t.Name == "Blood Pressure").FirstOrDefault().TestID,
                SequencePosition = 1
            };
            _context.Add(bptog1);
            _context.SaveChanges();
            TestGroup bptg1 = new TestGroup()
            {
                Name = "First Measure",
                SequencePosition = 1,
                TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID
            };
            TestGroup bptg2 = new TestGroup()
            {
                Name = "Second Measure",
                SequencePosition = 2,
                TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID
            };
            TestGroup bptg3 = new TestGroup()
            {
                Name = "Third Measure",
                SequencePosition = 3,
                TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID
            };
            _context.Add(bptg1);
            _context.Add(bptg2);
            _context.Add(bptg3);
            _context.SaveChanges();
            TestHeader Pulse1 = new TestHeader()
            {
                Name = "Pulse",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 1).FirstOrDefault().TestGroupID,
                SequencePosition = 1,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Pulse1);
            TestHeader Pulse2 = new TestHeader()
            {
                Name = "Pulse",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 2).FirstOrDefault().TestGroupID,
                SequencePosition = 1,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Pulse2);
            TestHeader Pulse3 = new TestHeader()
            {
                Name = "Pulse",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 3).FirstOrDefault().TestGroupID,
                SequencePosition = 1,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Pulse3);
            TestHeader Systolic1 = new TestHeader()
            {
                Name = "Systolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 1).FirstOrDefault().TestGroupID,
                SequencePosition = 2,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Systolic1);
            TestHeader Systolic2 = new TestHeader()
            {
                Name = "Systolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 2).FirstOrDefault().TestGroupID,
                SequencePosition = 2,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Systolic2);
            TestHeader Systolic3 = new TestHeader()
            {
                Name = "Systolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 3).FirstOrDefault().TestGroupID,
                SequencePosition = 2,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Systolic3);
            TestHeader Diastolic1 = new TestHeader()
            {
                Name = "Diastolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 1).FirstOrDefault().TestGroupID,
                SequencePosition = 3,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Diastolic1);
            TestHeader Diastolic2 = new TestHeader()
            {
                Name = "Diastolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 2).FirstOrDefault().TestGroupID,
                SequencePosition = 3,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Diastolic2);
            TestHeader Diastolic3 = new TestHeader()
            {
                Name = "Diastolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 3).FirstOrDefault().TestGroupID,
                SequencePosition = 3,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Diastolic3);
            _context.SaveChanges();
        }


        private async Task _4InsertECGTest()
        {
            var Test = new Test()
            {
                Name = "ECG",
                SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
            };
            _context.Add(Test);
            _context.SaveChanges();
            var testId = _context.Tests.Where(t => t.Name == "ECG").Select(t => t.TestID).FirstOrDefault();
            var experimentTest = new ExperimentTest()
            {
                TestID = testId,
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
            };
            var experimentTest2 = new ExperimentTest()
            {
                TestID = testId,
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
            };
            _context.Add(experimentTest);
            _context.Add(experimentTest2);
            _context.SaveChanges();
            var testoutergroup = new TestOuterGroup()
            {
                IsNone = true,
                TestID = testId,
                SequencePosition = 1
            };
            _context.Add(testoutergroup);
            _context.SaveChanges();
            var testgroup = new TestGroup()
            {
                IsNone = true,
                TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                    .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                SequencePosition = 1
            };
            _context.Add(testgroup);
            _context.SaveChanges();
            var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
            var avgHr = new TestHeader()
            {
                Name = "Average Hr",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 1,
                TestGroupID = tgId,
            };
            var sdnn = new TestHeader()
            {
                Name = "SDNN",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 2,
                TestGroupID = tgId,
            };
            var rnsdd = new TestHeader()
            {
                Name = "RNSDD",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 3,
                TestGroupID = tgId,
            };
            var sdsd = new TestHeader()
            {
                Name = "SDSD",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 4,
                TestGroupID = tgId,
            };
            var file = new TestHeader()
            {
                Name = "File",
                Type = AppUtility.DataTypeEnum.File.ToString(),
                SequencePosition = 5,
                TestGroupID = tgId,
            };
            _context.Add(avgHr);
            _context.Add(sdnn);
            _context.Add(rnsdd);
            _context.Add(sdsd);
            _context.Add(file);
            _context.SaveChanges();
        }

        private async Task _5InsertAnthropometryTest()
        {
            try
            {

                Test test = new Test()
                {
                    Name = "Anthropometry",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Anthropometry").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var weight = new TestHeader()
                {
                    Name = "Weight (kg)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                var height = new TestHeader()
                {
                    Name = "Height (cm)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                var bodyFat = new TestHeader()
                {
                    Name = "Body Fat (%)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 3,
                    TestGroupID = tgId,
                };
                var visceralFat = new TestHeader()
                {
                    Name = "Visceral Fat (%)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 4,
                    TestGroupID = tgId,
                };
                var muscleMass = new TestHeader()
                {
                    Name = "Muscle Mass (kg)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 5,
                    TestGroupID = tgId,
                };
                var skip = new TestHeader()
                {
                    IsSkip = true,
                    SequencePosition = 6,
                    TestGroupID = tgId
                };
                var waistCircumferance = new TestHeader()
                {
                    Name = "Waist Circumference (cm)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 7,
                    TestGroupID = tgId
                };
                var underBellyCircumferance = new TestHeader()
                {
                    Name = "Under Belly Circumference (cm)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 8,
                    TestGroupID = tgId
                };
                var bmi = new TestHeader()
                {
                    Name = "BMI",
                    Calculation = AppUtility.DataCalculation.BMI.ToString(),
                    SequencePosition = 9,
                    TestGroupID = tgId
                };
                _context.Update(weight);
                _context.Update(height);
                _context.Update(bodyFat);
                _context.Update(visceralFat);
                _context.Update(muscleMass);
                _context.Update(skip);
                _context.Update(waistCircumferance);
                _context.Update(underBellyCircumferance);
                _context.Update(bmi);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _6InsertFlexibilityTest()
        {
            Test test = new Test()
            {
                Name = "Flexibility",
                SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
            };
            _context.Add(test);
            _context.SaveChanges();
            var testId = _context.Tests.Where(t => t.Name == "Flexibility").Select(t => t.TestID).FirstOrDefault();
            var experimentTest = new ExperimentTest()
            {
                TestID = testId,
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
            };
            var experimentTest2 = new ExperimentTest()
            {
                TestID = testId,
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
            };
            _context.Add(experimentTest);
            _context.Add(experimentTest2);
            _context.SaveChanges();
            var testoutergroup = new TestOuterGroup()
            {
                IsNone = true,
                TestID = testId,
                SequencePosition = 1
            };
            _context.Add(testoutergroup);
            _context.SaveChanges();
            var togID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                         .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID;
            var testgroup1 = new TestGroup()
            {
                Name = "First Measure",
                TestOuterGroupID = togID,
                SequencePosition = 1
            };
            _context.Add(testgroup1);
            _context.SaveChanges();
            var testgroup2 = new TestGroup()
            {
                Name = "Second Measure",
                TestOuterGroupID = togID,
                SequencePosition = 2
            };
            _context.Add(testgroup2);
            _context.SaveChanges();
            var testgroup3 = new TestGroup()
            {
                Name = "Third Measure",
                TestOuterGroupID = togID,
                SequencePosition = 3
            };
            _context.Add(testgroup3);
            _context.SaveChanges();
            var tgId1 = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                     .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
            var tgId2 = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                     .Where(tg => tg.SequencePosition == 2).Select(tg => tg.TestGroupID).FirstOrDefault();
            var tgId3 = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                     .Where(tg => tg.SequencePosition == 3).Select(tg => tg.TestGroupID).FirstOrDefault();
            var distance1 = new TestHeader()
            {
                Name = "Distance from toes (cm)",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 1,
                TestGroupID = tgId1,
            };
            var distance2 = new TestHeader()
            {
                Name = "Distance from toes (cm)",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 2,
                TestGroupID = tgId2,
            };
            var distance3 = new TestHeader()
            {
                Name = "Distance from toes (cm)",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 3,
                TestGroupID = tgId3,
            };
            _context.Add(distance1);
            _context.Add(distance2);
            _context.Add(distance3);
            _context.SaveChanges();
        }

        private async Task _7InsertBalanceTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Balance",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Balance").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var eyesOpen = new TestOuterGroup()
                {
                    Name = "Eyes Open",
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(eyesOpen);
                var eyesClosed = new TestOuterGroup()
                {
                    Name = "Eyes Closed",
                    TestID = testId,
                    SequencePosition = 2
                };
                _context.Add(eyesClosed);
                _context.SaveChanges();
                var eyesOpenId = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                             .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID;
                var eyesClosedId = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                             .Where(tog => tog.SequencePosition == 2).FirstOrDefault().TestOuterGroupID;
                var eyesopenleftleg = new TestGroup()
                {
                    Name = "Left Leg",
                    TestOuterGroupID = eyesOpenId,
                    SequencePosition = 1
                };
                var eyesopenrightleg = new TestGroup()
                {
                    Name = "Right Leg",
                    TestOuterGroupID = eyesOpenId,
                    SequencePosition = 2
                };
                _context.Add(eyesopenleftleg);
                _context.Add(eyesopenrightleg);
                _context.SaveChanges();
                var eyesclosedleftleg = new TestGroup()
                {
                    Name = "Left Leg",
                    TestOuterGroupID = eyesClosedId,
                    SequencePosition = 1
                };
                var eyesclosedrightleg = new TestGroup()
                {
                    Name = "Right Leg",
                    TestOuterGroupID = eyesClosedId,
                    SequencePosition = 2
                };
                _context.Add(eyesclosedleftleg);
                _context.Add(eyesclosedrightleg);
                _context.SaveChanges();
                var eyesopenleftlegid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1 && tg.TestOuterGroupID == eyesOpenId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var eyesopenrightlegid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 2 && tg.TestOuterGroupID == eyesOpenId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var eyesclosedleftlegid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1 && tg.TestOuterGroupID == eyesClosedId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var eyesclosedrightlegid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 2 && tg.TestOuterGroupID == eyesClosedId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var firstmeasure1 = new TestHeader()
                {
                    Name = "First Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = eyesopenleftlegid,
                };
                var secondmeasure1 = new TestHeader()
                {
                    Name = "Second Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = eyesopenleftlegid,
                };
                var firstmeasure2 = new TestHeader()
                {
                    Name = "First Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = eyesopenrightlegid,
                };
                var secondmeasure2 = new TestHeader()
                {
                    Name = "Second Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = eyesopenrightlegid,
                };
                var firstmeasure3 = new TestHeader()
                {
                    Name = "First Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = eyesclosedleftlegid,
                };
                var secondmeasure3 = new TestHeader()
                {
                    Name = "Second Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = eyesclosedleftlegid,
                };
                var firstmeasure4 = new TestHeader()
                {
                    Name = "First Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = eyesclosedrightlegid,
                };
                var secondmeasure4 = new TestHeader()
                {
                    Name = "Second Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = eyesclosedrightlegid,
                };
                _context.Add(firstmeasure1);
                _context.Add(firstmeasure2);
                _context.Add(firstmeasure3);
                _context.Add(firstmeasure4);
                _context.Add(secondmeasure1);
                _context.Add(secondmeasure2);
                _context.Add(secondmeasure3);
                _context.Add(secondmeasure4);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _8InsertMuscleStrengthTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Muscle Strength",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Muscle Strength").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var handgrip = new TestOuterGroup()
                {
                    Name = "Handgrip",
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(handgrip);
                var legextension = new TestOuterGroup()
                {
                    Name = "Leg Extension",
                    TestID = testId,
                    SequencePosition = 2
                };
                _context.Add(legextension);
                _context.SaveChanges();
                var handgripId = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                             .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID;
                var legextesnionId = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                             .Where(tog => tog.SequencePosition == 2).FirstOrDefault().TestOuterGroupID;
                var lefthandhandgrip = new TestGroup()
                {
                    Name = "Left Hand",
                    TestOuterGroupID = handgripId,
                    SequencePosition = 1
                };
                var righthandhandgrip = new TestGroup()
                {
                    Name = "Right Hand",
                    TestOuterGroupID = handgripId,
                    SequencePosition = 2
                };
                _context.Add(lefthandhandgrip);
                _context.Add(righthandhandgrip);
                _context.SaveChanges();
                var leftleglegextension = new TestGroup()
                {
                    Name = "Left Leg",
                    TestOuterGroupID = legextesnionId,
                    SequencePosition = 1
                };
                var rightleglegextension = new TestGroup()
                {
                    Name = "Right Leg",
                    TestOuterGroupID = legextesnionId,
                    SequencePosition = 2
                };
                _context.Add(leftleglegextension);
                _context.Add(rightleglegextension);
                _context.SaveChanges();
                var lefthandhandgripid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1 && tg.TestOuterGroupID == handgripId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var righthandhandgripid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 2 && tg.TestOuterGroupID == handgripId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var leftleglegextensionid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1 && tg.TestOuterGroupID == legextesnionId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var rightleglegextensionid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 2 && tg.TestOuterGroupID == legextesnionId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var firstmeasure1 = new TestHeader()
                {
                    Name = "First Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = lefthandhandgripid,
                };
                var secondmeasure1 = new TestHeader()
                {
                    Name = "Second Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = lefthandhandgripid,
                };
                var firstmeasure2 = new TestHeader()
                {
                    Name = "First Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = righthandhandgripid,
                };
                var secondmeasure2 = new TestHeader()
                {
                    Name = "Second Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = righthandhandgripid,
                };
                var firstmeasure3 = new TestHeader()
                {
                    Name = "First Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = leftleglegextensionid,
                };
                var secondmeasure3 = new TestHeader()
                {
                    Name = "Second Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = leftleglegextensionid,
                };
                var firstmeasure4 = new TestHeader()
                {
                    Name = "First Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = rightleglegextensionid,
                };
                var secondmeasure4 = new TestHeader()
                {
                    Name = "Second Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = rightleglegextensionid,
                };
                _context.Add(firstmeasure1);
                _context.Add(firstmeasure2);
                _context.Add(firstmeasure3);
                _context.Add(firstmeasure4);
                _context.Add(secondmeasure1);
                _context.Add(secondmeasure2);
                _context.Add(secondmeasure3);
                _context.Add(secondmeasure4);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _9InsertCPETandSpirometryTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "CPET and Spirometry",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                await _context.SaveChangesAsync();
                var testId = _context.Tests.Where(t => t.Name == "CPET and Spirometry").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges(); var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file = new TestHeader()
                {
                    Name = "File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _10InsertDexaTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Dexa",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Dexa").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Results File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "Procedure File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _11InsertImmunochemistryTest()
        {
            try
            {

                Test test = new Test()
                {
                    Name = "Cognifit",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Cognifit").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Results File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "Procedure File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _12InsertBloodChemicalsTest()
        {
            try
            {

                Test test = new Test()
                {
                    Name = "Blood Chemicals",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Blood Chemicals").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Results File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "Procedure File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _13InsertBloodCountTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Blood Count",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Blood Count").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Results File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "Procedure File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _14InsertUltrasoundTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Ultrasound",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Ultrasound").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Echogardiagram File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "cIMT File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _15InsertAgeReaderTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Age Reader",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Age Reader").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var rightarm = new TestHeader()
                {
                    Name = "Right Arm",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(rightarm);
                await _context.SaveChangesAsync();
                var leftarm = new TestHeader()
                {
                    Name = "Left Arm",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(leftarm);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _16InsertProcedureDocTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Procedure Document",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Procedure Document").Select(t => t.TestID).FirstOrDefault();
                var experimentTest = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
                };
                var experimentTest2 = new ExperimentTest()
                {
                    TestID = testId,
                    ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
                };
                _context.Add(experimentTest);
                _context.Add(experimentTest2);
                _context.SaveChanges();
                var testoutergroup = new TestOuterGroup()
                {
                    IsNone = true,
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(testoutergroup);
                _context.SaveChanges();
                var testgroup = new TestGroup()
                {
                    IsNone = true,
                    TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                        .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                    SequencePosition = 1
                };
                _context.Add(testgroup);
                _context.SaveChanges();
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var procDoc = new TestHeader()
                {
                    Name = "File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(procDoc);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
