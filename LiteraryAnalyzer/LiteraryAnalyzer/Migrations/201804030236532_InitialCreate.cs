namespace LiteraryAnalyzer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Excerpts",
                c => new
                    {
                        ExcerptID = c.Int(nullable: false, identity: true),
                        ExcerptText = c.String(),
                        Excerpt_ExcerptID = c.Int(),
                    })
                .PrimaryKey(t => t.ExcerptID)
                .ForeignKey("dbo.Excerpts", t => t.Excerpt_ExcerptID)
                .Index(t => t.Excerpt_ExcerptID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Excerpts", "Excerpt_ExcerptID", "dbo.Excerpts");
            DropIndex("dbo.Excerpts", new[] { "Excerpt_ExcerptID" });
            DropTable("dbo.Excerpts");
        }
    }
}
