using System;
using System.Collections.Generic;
using System.Linq;

namespace epi.TestFramework
{
    public class TestFailure : Exception
    {
        private IList<Property> properties;

        public enum PropertyName
        {
            EXCEPTION_MESSAGE, // message of unhandled exception
            EXPLANATION,       // explanation from TSV file
            COMMAND,  // last command, that caused the error, in API-testing programs
            STATE,    // state of the user-defined collection (for instance, in API
                      // testing)
            EXPECTED, // expected result
            RESULT,   // user-produced result
            MISSING_ITEMS,  // list of items from input that are missing in the result
                            // set
            EXCESS_ITEMS,   // list of items from result that are missing in the input
                            // set
            MISMATCH_INDEX, // for collections: index of the wrong item in result
                            // for binary trees: instance of TreePath describing the
                            // position of the wrong item
            EXPECTED_ITEM,  // value of the expected item in collection (not the whole
                            // collection)
            RESULT_ITEM     // value of the result item in collection (not the whole
                            // collection)
        }

        public class Property
        {
            public Property(PropertyName name, object value)
            {
                RawName = name;
                Value = value;
            }

            public string Name { get => RawName.ToString().ToLower().Replace('_', ' '); }
            public PropertyName RawName { get; }
            public int ID { get => (int)RawName; }
            public object Value { get; }
        }

        public TestFailure(string description = "") : base(description)
        {
            Properties = new List<Property>();
            Description = description;
        }

        public TestFailure WithProperty(PropertyName name, object value)
        {
            Properties.Add(new Property(name, value));
            return this;
        }

        public TestFailure WithMismatchInfo(Object mismatchIndex,
            Object expectedItem, Object resultItem)
        {
            return WithProperty(PropertyName.MISMATCH_INDEX, mismatchIndex)
                .WithProperty(PropertyName.EXPECTED_ITEM, expectedItem)
                .WithProperty(PropertyName.RESULT_ITEM, resultItem);
        }

        public string Description { get; }
        public IList<Property> Properties
        {
            get => properties = properties.OrderBy(p => p.ID).ToList();
            private set => properties = value;
        }

        public int MaxPropertyNameLength
        {
            get => Properties
                .Select(property => property.Name.Length)?
                .Max()
                ?? 0;
        }
    }
}
