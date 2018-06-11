namespace LiteraryAnalyzer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LitelmRefactor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Excerpts", "Excerpt_ExcerptID", "dbo.Excerpts");
            DropForeignKey("dbo.Excerpts", "TokenID", "dbo.Tokens");
            DropIndex("dbo.Excerpts", new[] { "TokenID" });
            DropIndex("dbo.Excerpts", new[] { "Excerpt_ExcerptID" });
            CreateTable(
                "dbo.Litelms",
                c => new
                    {
                        LitelmID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Index = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Source_LitelmID = c.Int(),
                        LitHeader_LitelmID = c.Int(),
                    })
                .PrimaryKey(t => t.LitelmID)
                .ForeignKey("dbo.Litelms", t => t.Source_LitelmID)
                .ForeignKey("dbo.Litelms", t => t.LitHeader_LitelmID)
                .Index(t => t.Source_LitelmID)
                .Index(t => t.LitHeader_LitelmID);
            
            DropTable("dbo.ExceptionLogs");
            DropTable("dbo.Excerpts");
            DropTable("dbo.Tokens");
            DropTable("dbo.MarkdownOptions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MarkdownOptions",
                c => new
                    {
                        MarkdownOptionID = c.Int(nullable: false, identity: true),
                        URIOption = c.Int(nullable: false),
                        ContentsOption = c.Int(nullable: false),
                        ParserOption = c.Int(nullable: false),
                        BaseDir = c.String(),
                        Filename = c.String(),
                        Prefix = c.String(),
                    })
                .PrimaryKey(t => t.MarkdownOptionID);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        TokenID = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.TokenID);
            
            CreateTable(
                "dbo.Excerpts",
                c => new
                    {
                        ExcerptID = c.Int(nullable: false, identity: true),
                        ExcerptText = c.String(),
                        TokenID = c.String(maxLength: 128),
                        Excerpt_ExcerptID = c.Int(),
                    })
                .PrimaryKey(t => t.ExcerptID);
            
            CreateTable(
                "dbo.ExceptionLogs",
                c => new
                    {
                        ExceptionLogID = c.Int(nullable: false, identity: true),
                        StackTrace = c.String(),
                        Message = c.String(),
                        Source = c.String(),
                        ExToString = c.String(),
                        Commentary = c.String(),
                        OccuranceTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ExceptionLogID);
            
            DropForeignKey("dbo.Litelms", "LitHeader_LitelmID", "dbo.Litelms");
            DropForeignKey("dbo.Litelms", "Source_LitelmID", "dbo.Litelms");
            DropIndex("dbo.Litelms", new[] { "LitHeader_LitelmID" });
            DropIndex("dbo.Litelms", new[] { "Source_LitelmID" });
            DropTable("dbo.Litelms");
            CreateIndex("dbo.Excerpts", "Excerpt_ExcerptID");
            CreateIndex("dbo.Excerpts", "TokenID");
            AddForeignKey("dbo.Excerpts", "TokenID", "dbo.Tokens", "TokenID");
            AddForeignKey("dbo.Excerpts", "Excerpt_ExcerptID", "dbo.Excerpts", "ExcerptID");
        }
    }
}
