using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class TestValuesProc : ApplicationDbContextProc<TestValue>
    {
        public TestValuesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                this.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> SaveAsync(TestViewModel TestViewModel)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                foreach (var fieldTest in TestViewModel.FieldViewModels)
                {
                    TestValue testValue = new TestValue();
                    if (fieldTest.TestValueID != 0)
                    {
                        testValue = await _testValuesProc.ReadOne(new List<Expression<Func<TestValue, bool>>> 
                                                            { tv => tv.TestValueID == fieldTest.TestValueID });
                    }
                    else
                    {
                        testValue.ExperimentEntryID = TestViewModel.ExperimentEntry.ExperimentEntryID;
                        testValue.TestHeaderID = fieldTest.TestHeader.TestHeaderID;
                    }
                    switch (fieldTest.DataTypeEnum)
                    {
                        case AppUtility.DataTypeEnum.Double:
                            testValue.Value = fieldTest.Double.ToString();
                            break;
                        case AppUtility.DataTypeEnum.String:
                            testValue.Value = fieldTest.String;
                            break;
                        case AppUtility.DataTypeEnum.DateTime:
                            testValue.Value = fieldTest.DateTime.ToString();
                            break;
                        case AppUtility.DataTypeEnum.Bool:
                            testValue.Value = fieldTest.Bool.ToString();
                            break;
                        case AppUtility.DataTypeEnum.File:
                            //save file here and save name of file
                            if (fieldTest.File != null)
                            {
                                testValue.Value = fieldTest.File.ToString();
                            }
                            break;
                    }
                    _context.Update(testValue);
                }
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch(Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public async Task<StringWithBool> CreateEmptyTestValuesForTests(List<Test> tests, List<TestValue> testValues, int ExperimentEntryID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var allTests = tests.SelectMany(t => t.TestOuterGroups.SelectMany(tog => tog.TestGroups.SelectMany(tg => tg.TestHeaders)));
                foreach (var testheader in tests.SelectMany(t => t.TestOuterGroups.SelectMany(tog => tog.TestGroups.SelectMany(tg => tg.TestHeaders))))
                {
                    if (!testValues.Where(tv => tv.TestHeaderID == testheader.TestHeaderID).Any())
                    {
                        TestValue tv = new TestValue()
                        {
                            TestHeaderID = testheader.TestHeaderID,
                            ExperimentEntryID = ExperimentEntryID
                        };
                        _context.Update(tv);

                        testValues.Add(tv);
                    }
                }
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

    }
}
