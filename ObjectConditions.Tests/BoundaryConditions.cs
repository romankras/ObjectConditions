using NUnit.Framework;
using Sprache;

namespace ObjectConditions.Tests
{
    [TestFixture]
    public class BoundaryConditions
    {
        [Test]
        public void NestedExpressionsTest()
        {
            const string str = "!(!(true   =   ConfigValue::dvfyBPS4   )   =   ConfigValue::dvfyBPS4   )";

            var ast = new BinaryRelation()
            {
                IsNegated = true,
                Left = new BinaryRelation()
                {
                    IsNegated = true,
                    Left = new ObjectValue()
                    {
                        IsNegated = false,
                        Value = "true"
                    },
                    Operator = BinaryOperators.Equality,
                    Right = new TypedObject()
                    {
                        IsNegated = false,
                        Name = "dvfyBPS4",
                        ObjectType = "ConfigValue"
                    }
                },
                Operator = BinaryOperators.Equality,
                Right = new TypedObject()
                {
                    IsNegated = false,
                    Name = "dvfyBPS4",
                    ObjectType = "ConfigValue"
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void OrdinaryBinaryRelationTest()
        {
            const string str = "( Yga9SELk < MDJoNveF::XtuJduTe)";

            var ast = new BinaryRelation()
            {
                IsNegated = false,
                Left = new ObjectValue()
                {
                    IsNegated = false,
                    Value = "Yga9SELk"
                },
                Operator = BinaryOperators.LessThan,
                Right = new TypedObject()
                {
                    IsNegated = false,
                    Name = "XtuJduTe",
                    ObjectType = "MDJoNveF"
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);

            const string str2 = "3O3MDn2I >!   Hmys6L2I::Df4A1pzX";

            var ast2 = new BinaryRelation()
            {
                IsNegated = false,
                Left = new ObjectValue()
                {
                    IsNegated = false,
                    Value = "3O3MDn2I"
                },
                Operator = BinaryOperators.GreaterThan,
                Right = new TypedObject()
                {
                    IsNegated = true,
                    Name = "Df4A1pzX",
                    ObjectType = "Hmys6L2I"
                }
            };

            var parsed2 = LanguageGrammar.ParseExpression.Parse(str2);
            Assert.AreEqual(ast2, parsed2);
        }

        [Test]
        public void NestingOrderTest()
        {
            const string str = "(GeeNBdZD <= !YkNnEypc::SfE3OcCH And yVcxvXwH)";

            var ast = new BinaryRelation()
            {
                IsNegated = false,
                Left = new ObjectValue()
                {
                    IsNegated = false,
                    Value = "GeeNBdZD"
                },
                Operator = BinaryOperators.LessOrEqual,
                Right = new BinaryRelation()
                {
                    IsNegated = false,
                    Left = new TypedObject()
                    {
                        IsNegated = true,
                        Name = "SfE3OcCH",
                        ObjectType = "YkNnEypc"
                    },
                    Operator = BinaryOperators.Conjunction,
                    Right = new ObjectValue()
                    {
                        IsNegated = false,
                        Value = "yVcxvXwH"
                    }
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void DoubleNegationTest()
        {
            const string str = "!!VFcsfyxF::V2dnlJGW < 1yjlGOQR";

            var ast = new BinaryRelation()
            {
                IsNegated = false,
                Left = new TypedObject()
                {
                    IsNegated = false,
                    Name = "V2dnlJGW",
                    ObjectType = "VFcsfyxF"
                },
                Operator = BinaryOperators.LessThan,
                Right = new ObjectValue()
                {
                    IsNegated = false,
                    Value = "1yjlGOQR"
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void ObjectCountTest()
        {
            var ast = new BinaryRelation()
            {
                IsNegated = false,
                Left = new TypedObject(),
                Operator = BinaryOperators.LessThan,
                Right = new ObjectValue()
            };

            Assert.AreEqual(2, ast.Children.Count);
            Assert.AreEqual(0, ast.Children[0].Children.Count);
            Assert.AreEqual(0, ast.Children[1].Children.Count);

            ast = new BinaryRelation()
            {
                IsNegated = true,
                Left = new BinaryRelation()
                {
                    IsNegated = true,
                    Left = new ObjectValue()
                    {
                        IsNegated = false,
                        Value = "value1"
                    },
                    Operator = BinaryOperators.Equality,
                    Right = new TypedObject()
                    {
                        IsNegated = false,
                        Name = "object1",
                        ObjectType = "type1"
                    }
                },
                Operator = BinaryOperators.Equality,
                Right = new TypedObject()
                {
                    IsNegated = false,
                    Name = "object2",
                    ObjectType = "type2"
                }
            };

            var flat = Helper.GetFlatTree(ast);
            Assert.AreEqual(5, flat.Count);
        }

        [Test]
        public void EmptyObjectsTest()
        {
            var ast = new BinaryRelation()
            {
                IsNegated = true,
                Left = new BinaryRelation()
                {
                    IsNegated = true,
                    Left = new ObjectValue(),
                    Operator = BinaryOperators.Equality,
                    Right = new TypedObject()
                },
                Operator = BinaryOperators.Equality,
                Right = new TypedObject()
            };

            var flat = Helper.GetFlatTree(ast);
            Assert.AreEqual(5, flat.Count);
        }
    }
}
