using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteraryAnalyzer {
	public class LiteraryAnalyzerContext : DbContext {
		public DbSet<Excerpt> Excerpts { get; set; }
		public DbSet<Token> Tokens { get; set; }
		public DbSet<ExceptionLog> ExceptionLogs { get; set; }
		public DbSet<MarkdownOption> MarkdownOptions { get; set; }

		/// <summary>
		/// A way to garuntee retrieval of a token with t.TokenID == TokenID
		/// </summary>
		/// <param name="TokenID">The TokenID to find</param>
		/// <returns>A Token t where t.TokenID == TokenID</returns>
		public Token GetTokenWithWrite(String TokenID) {
			var query = this.Tokens.Where(t => t.TokenID.Equals(TokenID));
			if (query.Count() == 0) {
				var contentToken = new Token { TokenID = TokenID };
				this.Tokens.Add(contentToken);
				this.SaveChanges();
				return contentToken;
			}
			else {
				return query.First();
			}
		}
	}
}
