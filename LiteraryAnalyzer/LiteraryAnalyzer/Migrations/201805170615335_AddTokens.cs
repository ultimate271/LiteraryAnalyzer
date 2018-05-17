namespace LiteraryAnalyzer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTokens : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        TokenID = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.TokenID);
            
            AddColumn("dbo.Excerpts", "TokenID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Excerpts", "TokenID");
            AddForeignKey("dbo.Excerpts", "TokenID", "dbo.Tokens", "TokenID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Excerpts", "TokenID", "dbo.Tokens");
            DropIndex("dbo.Excerpts", new[] { "TokenID" });
            DropColumn("dbo.Excerpts", "TokenID");
            DropTable("dbo.Tokens");
        }
    }
}
