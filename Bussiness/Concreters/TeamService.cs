using Bussiness.Abstracts;
using Bussiness.Exceptions;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Concreters
{
	public class TeamService : ITeamService
	{
		ITeamRepository _teamRepository;
		IWebHostEnvironment _webHostEnvironment;

		public TeamService(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment)
		{
			_teamRepository = teamRepository;
			_webHostEnvironment = webHostEnvironment;
		}

		public void Create(Team team)
		{
			if(team == null) {
				throw new NotFoundException("Bele bir obyekt yoxdur!");
			}
			if(team.PhotoFile==null)
			{
				throw new NotNullException("PhotoFile", "Sekli bos gondermek olmaz!");
			}
			if (!team.PhotoFile.ContentType.Contains("image/"))
			{
				throw new FileContentTypeException("PhotoFile", "File tipi duzgun deyil!");
			}
			if (team.PhotoFile.Length > 2097152)
			{
				throw new FileSizeException("PhotoFile", "File olcusu duzgun deyil!");
			}
			string filename=team.PhotoFile.FileName;
			string path = _webHostEnvironment.WebRootPath + @"\Upload\Team\" + filename;
			using(FileStream file=new FileStream(path, FileMode.Create))
			{
				team.PhotoFile.CopyTo(file);
			}
			team.ImgUrl = filename;
			_teamRepository.Add(team);
			_teamRepository.Commit();
		}

		public void Delete(int id)
		{
			var deleteTeam=_teamRepository.Get(x=> x.Id == id);	
			if(deleteTeam == null) { 
				throw new NotFoundException("Bele bir obyekt yoxdur!");

			}
			string path = _webHostEnvironment.WebRootPath + @"\Upload\Team\" + deleteTeam.ImgUrl;
			FileInfo fileInfo = new FileInfo(path);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			_teamRepository.Delete(deleteTeam);
			_teamRepository.Commit();

		}

		public List<Team> GetAllTeams(Func<Team, bool>? func = null)
		{
			return _teamRepository.GetAll(func);
		}

		public Team GetTeam(Func<Team, bool>? func = null)
		{
			return _teamRepository.Get(func);
		}

		public void Update(int id, Team team)
		{
			var updateTeam=_teamRepository.Get(x=> x.Id == id);	
			if(updateTeam == null)
			{
				throw new NotFoundException("Bele bir obyekt yoxdur!");

			}
			if(team.PhotoFile != null)
			{
				if (!team.PhotoFile.ContentType.Contains("image/"))
				{
					throw new FileContentTypeException("PhotoFile", "File tipi duzgun deyil!");
				}
				if (team.PhotoFile.Length > 2097152)
				{
					throw new FileSizeException("PhotoFile", "File olcusu duzgun deyil!");
				}
				string filename = team.PhotoFile.FileName;
				string path = _webHostEnvironment.WebRootPath + @"\Upload\Team\" + filename;
				using (FileStream file = new FileStream(path, FileMode.Create))
				{
					team.PhotoFile.CopyTo(file);
				}
				updateTeam.ImgUrl = filename;
			}
			else
			{
				team.ImgUrl=updateTeam.ImgUrl;
			}
			updateTeam.Name = team.Name;
			updateTeam.Position = team.Position;
			_teamRepository.Commit();
		}
	}
}
