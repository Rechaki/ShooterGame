//using System;
//using System.IO;
//using UnityEngine;

//// Token: 0x0200009A RID: 154
//public class SaveManager : Singleton<SaveManager>
//{
//	// Token: 0x0600064F RID: 1615 RVA: 0x00034CF0 File Offset: 0x00032EF0
//	void Awake()
//	{
//		SaveManager.mgr = this;
//		if (Application.isEditor)
//		{
//			this.LoadSaveData();
//		}
//	}

//	// Token: 0x06000650 RID: 1616 RVA: 0x00034D0C File Offset: 0x00032F0C
//	public void LoadSaveData()
//	{
//		if (!File.Exists("save.json") || JsonUtility.FromJson<save>(File.ReadAllText("save.json")).ver == 0)
//		{
//			SaveManager.save = new save();
//			SaveManager.save.ver = 1;
//			File.WriteAllText("save.json", JsonUtility.ToJson(SaveManager.save));
//			return;
//		}
//		SaveManager.save = JsonUtility.FromJson<save>(File.ReadAllText("save.json"));
//	}

//	// Token: 0x06000651 RID: 1617 RVA: 0x00034D79 File Offset: 0x00032F79
//	public void ToggleIsVisualAssisting(bool toggle)
//	{
//		SaveManager.save.isVisualAssisting = toggle;
//		this.SaveGame();
//	}

//	// Token: 0x06000652 RID: 1618 RVA: 0x00034D8C File Offset: 0x00032F8C
//	public void ToggleAudioAssisting(bool toggle)
//	{
//		SaveManager.save.isAudioAssisting = toggle;
//		this.SaveGame();
//	}

//	// Token: 0x06000653 RID: 1619 RVA: 0x00034D9F File Offset: 0x00032F9F
//	public void ToggleIsScreenShakeDisabled(bool toggle)
//	{
//		SaveManager.save.isScreenShakeDisabled = toggle;
//		this.SaveGame();
//	}

//	// Token: 0x06000654 RID: 1620 RVA: 0x00034DB2 File Offset: 0x00032FB2
//	public void ToggleIsGameComplete(bool toggle)
//	{
//		SaveManager.save.igc = toggle;
//		this.SaveGame();
//	}

//	// Token: 0x06000655 RID: 1621 RVA: 0x00034DC5 File Offset: 0x00032FC5
//	public void SetLang(int newLang)
//	{
//		SaveManager.save.lang = newLang;
//		this.SaveGame();
//	}

//	// Token: 0x06000656 RID: 1622 RVA: 0x00034DD8 File Offset: 0x00032FD8
//	public void SetCalibrationOffset(int newCalibrationOffset)
//	{
//		SaveManager.save.calibrationOffset = newCalibrationOffset;
//		this.SaveGame();
//	}

//	// Token: 0x06000657 RID: 1623 RVA: 0x00034DEB File Offset: 0x00032FEB
//	public void SetMaster(int newVolume)
//	{
//		SaveManager.save.master = newVolume;
//		this.SaveGame();
//	}

//	// Token: 0x06000658 RID: 1624 RVA: 0x00034DFE File Offset: 0x00032FFE
//	public void SetMusic(int newVolume)
//	{
//		SaveManager.save.music = newVolume;
//		this.SaveGame();
//	}

//	// Token: 0x06000659 RID: 1625 RVA: 0x00034E11 File Offset: 0x00033011
//	public void SetSfx(int newVolume)
//	{
//		SaveManager.save.sfx = newVolume;
//		this.SaveGame();
//	}

//	// Token: 0x0600065A RID: 1626 RVA: 0x00034E24 File Offset: 0x00033024
//	public void SetMetronome(int newVolume)
//	{
//		SaveManager.save.metronome = newVolume;
//		this.SaveGame();
//	}

//	// Token: 0x0600065B RID: 1627 RVA: 0x00034E37 File Offset: 0x00033037
//	public void SetChapterNum(int newChapterNum)
//	{
//		SaveManager.save.cn = newChapterNum;
//		this.SaveGame();
//	}

//	// Token: 0x0600065C RID: 1628 RVA: 0x00034E4C File Offset: 0x0003304C
//	public void SetScore(string dreamName, int newScore)
//	{
//		if (dreamName == "Dream_food")
//		{
//			SaveManager.save.fd = newScore;
//		}
//		else if (dreamName == "Dream_foodAlt")
//		{
//			SaveManager.save.fdAlt = newScore;
//		}
//		else if (dreamName == "Dream_shopping")
//		{
//			SaveManager.save.sp = newScore;
//		}
//		else if (dreamName == "Dream_shoppingAlt")
//		{
//			SaveManager.save.spAlt = newScore;
//		}
//		else if (dreamName == "Dream_tech")
//		{
//			SaveManager.save.tc = newScore;
//		}
//		else if (dreamName == "Dream_techAlt")
//		{
//			SaveManager.save.tcAlt = newScore;
//		}
//		else if (dreamName == "Dream_followers")
//		{
//			SaveManager.save.fw = newScore;
//		}
//		else if (dreamName == "Dream_followersAlt")
//		{
//			SaveManager.save.fwAlt = newScore;
//		}
//		else if (dreamName == "Dream_indulgence")
//		{
//			SaveManager.save.id = newScore;
//		}
//		else if (dreamName == "Dream_indulgenceAlt")
//		{
//			SaveManager.save.idAlt = newScore;
//		}
//		else if (dreamName == "Dream_exercise")
//		{
//			SaveManager.save.ec = newScore;
//		}
//		else if (dreamName == "Dream_exerciseAlt")
//		{
//			SaveManager.save.ecAlt = newScore;
//		}
//		else if (dreamName == "Dream_career")
//		{
//			SaveManager.save.cr = newScore;
//		}
//		else if (dreamName == "Dream_careerAlt")
//		{
//			SaveManager.save.crAlt = newScore;
//		}
//		else if (dreamName == "Dream_money")
//		{
//			SaveManager.save.sv = newScore;
//		}
//		else if (dreamName == "Dream_moneyAlt")
//		{
//			SaveManager.save.svAlt = newScore;
//		}
//		else if (dreamName == "Dream_dating")
//		{
//			SaveManager.save.dt = newScore;
//		}
//		else if (dreamName == "Dream_datingAlt")
//		{
//			SaveManager.save.dtAlt = newScore;
//		}
//		else if (dreamName == "Dream_pressure")
//		{
//			SaveManager.save.pr = newScore;
//		}
//		else if (dreamName == "Dream_pressureAlt")
//		{
//			SaveManager.save.prAlt = newScore;
//		}
//		else if (dreamName == "Dream_time")
//		{
//			SaveManager.save.ft = newScore;
//		}
//		else if (dreamName == "Dream_timeAlt")
//		{
//			SaveManager.save.ftAlt = newScore;
//		}
//		else if (dreamName == "Dream_mind")
//		{
//			SaveManager.save.mn = newScore;
//		}
//		else if (dreamName == "Dream_mindAlt")
//		{
//			SaveManager.save.mnAlt = newScore;
//		}
//		else if (dreamName == "Dream_space")
//		{
//			SaveManager.save.wr = newScore;
//		}
//		else if (dreamName == "Dream_spaceAlt")
//		{
//			SaveManager.save.wrAlt = newScore;
//		}
//		else if (dreamName == "Dream_nature")
//		{
//			SaveManager.save.nr = newScore;
//		}
//		else if (dreamName == "Dream_natureAlt")
//		{
//			SaveManager.save.nrAlt = newScore;
//		}
//		else if (dreamName == "Dream_meditation")
//		{
//			SaveManager.save.md = newScore;
//		}
//		else if (dreamName == "Dream_meditationAlt")
//		{
//			SaveManager.save.mdAlt = newScore;
//		}
//		else if (dreamName == "Dream_stress")
//		{
//			SaveManager.save.fr = newScore;
//		}
//		else if (dreamName == "Dream_stressAlt")
//		{
//			SaveManager.save.frAlt = newScore;
//		}
//		else if (dreamName == "Dream_desires")
//		{
//			SaveManager.save.me = newScore;
//		}
//		else if (dreamName == "Dream_desiresAlt")
//		{
//			SaveManager.save.meAlt = newScore;
//		}
//		else if (dreamName == "Dream_past")
//		{
//			SaveManager.save.ps = newScore;
//		}
//		else if (dreamName == "Dream_pastAlt")
//		{
//			SaveManager.save.psAlt = newScore;
//		}
//		else if (dreamName == "Dream_future")
//		{
//			SaveManager.save.fu = newScore;
//		}
//		else if (dreamName == "Dream_futureAlt")
//		{
//			SaveManager.save.fuAlt = newScore;
//		}
//		else if (dreamName == "Dream_anxiety")
//		{
//			SaveManager.save.ax = newScore;
//		}
//		else if (dreamName == "Dream_anxietyAlt")
//		{
//			SaveManager.save.axAlt = newScore;
//		}
//		else if (dreamName == "Dream_final")
//		{
//			SaveManager.save.fn = newScore;
//		}
//		else if (dreamName == "Dream_finalAlt")
//		{
//			SaveManager.save.fnAlt = newScore;
//		}
//		this.SaveGame();
//	}

//	// Token: 0x0600065D RID: 1629 RVA: 0x00035310 File Offset: 0x00033510
//	public void ClearSaveData()
//	{
//		if (File.Exists("save.json"))
//		{
//			File.Delete("save.json");
//		}
//		SaveManager.timeSaved = 0f;
//		PlayerPrefs.DeleteAll();
//		this.LoadSaveData();
//	}

//	// Token: 0x0600065E RID: 1630 RVA: 0x00035340 File Offset: 0x00033540
//	public void SaveGame()
//	{
//		SaveManager.save.min = SaveManager.save.min + (Time.unscaledTime - SaveManager.timeSaved) / 60f;
//		SaveManager.timeSaved = Time.unscaledTime;
//		MonoBehaviour.print(SaveManager.save.min.ToString() + " min(s) played");
//		File.WriteAllText("save.json", JsonUtility.ToJson(SaveManager.save));
//	}

//	// Token: 0x0600065F RID: 1631 RVA: 0x000353AF File Offset: 0x000335AF
//	private int ConvertScoreToEarned(int starScore)
//	{
//		if (starScore < 4)
//		{
//			return starScore;
//		}
//		return 3;
//	}

//	// Token: 0x06000660 RID: 1632 RVA: 0x000353B8 File Offset: 0x000335B8
//	public bool CheckIsGameComplete()
//	{
//		return SaveManager.save.igc;
//	}

//	// Token: 0x06000661 RID: 1633 RVA: 0x000353C4 File Offset: 0x000335C4
//	public bool CheckIsVisualAssisting()
//	{
//		return SaveManager.save.isVisualAssisting;
//	}

//	// Token: 0x06000662 RID: 1634 RVA: 0x000353D0 File Offset: 0x000335D0
//	public bool CheckIsAudioAssisting()
//	{
//		return SaveManager.save.isAudioAssisting;
//	}

//	// Token: 0x06000663 RID: 1635 RVA: 0x000353DC File Offset: 0x000335DC
//	public bool CheckIsScreenShakeDisabled()
//	{
//		return SaveManager.save.isScreenShakeDisabled;
//	}

//	// Token: 0x06000664 RID: 1636 RVA: 0x000353E8 File Offset: 0x000335E8
//	public int GetLang()
//	{
//		return SaveManager.save.lang;
//	}

//	// Token: 0x06000665 RID: 1637 RVA: 0x000353F4 File Offset: 0x000335F4
//	public int GetCalibrationOffset()
//	{
//		return SaveManager.save.calibrationOffset;
//	}

//	// Token: 0x06000666 RID: 1638 RVA: 0x00035400 File Offset: 0x00033600
//	public int GetMaster()
//	{
//		return SaveManager.save.master;
//	}

//	// Token: 0x06000667 RID: 1639 RVA: 0x0003540C File Offset: 0x0003360C
//	public int GetMusic()
//	{
//		return SaveManager.save.music;
//	}

//	// Token: 0x06000668 RID: 1640 RVA: 0x00035418 File Offset: 0x00033618
//	public int GetSfx()
//	{
//		return SaveManager.save.sfx;
//	}

//	// Token: 0x06000669 RID: 1641 RVA: 0x00035424 File Offset: 0x00033624
//	public int GetMetronome()
//	{
//		return SaveManager.save.metronome;
//	}

//	// Token: 0x0600066A RID: 1642 RVA: 0x00035430 File Offset: 0x00033630
//	public int GetChapterNum()
//	{
//		return SaveManager.save.cn;
//	}

//	// Token: 0x0600066B RID: 1643 RVA: 0x0003543C File Offset: 0x0003363C
//	public int GetChapterEarnedStars(int chapterNum)
//	{
//		if (chapterNum == 1)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.fd) + this.ConvertScoreToEarned(SaveManager.save.sp) + this.ConvertScoreToEarned(SaveManager.save.tc) + this.ConvertScoreToEarned(SaveManager.save.fw) + this.ConvertScoreToEarned(SaveManager.save.id);
//		}
//		if (chapterNum == 2)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.ec) + this.ConvertScoreToEarned(SaveManager.save.cr) + this.ConvertScoreToEarned(SaveManager.save.sv) + this.ConvertScoreToEarned(SaveManager.save.dt) + this.ConvertScoreToEarned(SaveManager.save.pr);
//		}
//		if (chapterNum == 3)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.ft) + this.ConvertScoreToEarned(SaveManager.save.mn) + this.ConvertScoreToEarned(SaveManager.save.wr) + this.ConvertScoreToEarned(SaveManager.save.nr) + this.ConvertScoreToEarned(SaveManager.save.md);
//		}
//		if (chapterNum == 4)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.fr) + this.ConvertScoreToEarned(SaveManager.save.me) + this.ConvertScoreToEarned(SaveManager.save.ps) + this.ConvertScoreToEarned(SaveManager.save.fu) + this.ConvertScoreToEarned(SaveManager.save.ax);
//		}
//		if (chapterNum == 5)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.fn);
//		}
//		return 0;
//	}

//	// Token: 0x0600066C RID: 1644 RVA: 0x000355C4 File Offset: 0x000337C4
//	private int GetChapterEarnedRings(int chapterNum)
//	{
//		if (chapterNum == 1)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.fdAlt) + this.ConvertScoreToEarned(SaveManager.save.spAlt) + this.ConvertScoreToEarned(SaveManager.save.tcAlt) + this.ConvertScoreToEarned(SaveManager.save.fwAlt) + this.ConvertScoreToEarned(SaveManager.save.idAlt);
//		}
//		if (chapterNum == 2)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.ecAlt) + this.ConvertScoreToEarned(SaveManager.save.crAlt) + this.ConvertScoreToEarned(SaveManager.save.svAlt) + this.ConvertScoreToEarned(SaveManager.save.dtAlt) + this.ConvertScoreToEarned(SaveManager.save.prAlt);
//		}
//		if (chapterNum == 3)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.ftAlt) + this.ConvertScoreToEarned(SaveManager.save.mnAlt) + this.ConvertScoreToEarned(SaveManager.save.wrAlt) + this.ConvertScoreToEarned(SaveManager.save.nrAlt) + this.ConvertScoreToEarned(SaveManager.save.mdAlt);
//		}
//		if (chapterNum == 4)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.frAlt) + this.ConvertScoreToEarned(SaveManager.save.meAlt) + this.ConvertScoreToEarned(SaveManager.save.psAlt) + this.ConvertScoreToEarned(SaveManager.save.fuAlt) + this.ConvertScoreToEarned(SaveManager.save.axAlt);
//		}
//		if (chapterNum == 5)
//		{
//			return this.ConvertScoreToEarned(SaveManager.save.fnAlt);
//		}
//		return 0;
//	}

//	// Token: 0x0600066D RID: 1645 RVA: 0x0003574B File Offset: 0x0003394B
//	public int GetTotalEarnedStars()
//	{
//		return this.GetChapterEarnedStars(1) + this.GetChapterEarnedStars(2) + this.GetChapterEarnedStars(3) + this.GetChapterEarnedStars(4) + this.GetChapterEarnedStars(5);
//	}

//	// Token: 0x0600066E RID: 1646 RVA: 0x00035774 File Offset: 0x00033974
//	public int GetTotalEarnedRings()
//	{
//		return this.GetChapterEarnedRings(1) + this.GetChapterEarnedRings(2) + this.GetChapterEarnedRings(3) + this.GetChapterEarnedRings(4) + this.GetChapterEarnedRings(5);
//	}

//	// Token: 0x0600066F RID: 1647 RVA: 0x000357A0 File Offset: 0x000339A0
//	public int GetScore(string dreamName)
//	{
//		if (dreamName == "Dream_food")
//		{
//			return SaveManager.save.fd;
//		}
//		if (dreamName == "Dream_foodAlt")
//		{
//			return SaveManager.save.fdAlt;
//		}
//		if (dreamName == "Dream_shopping")
//		{
//			return SaveManager.save.sp;
//		}
//		if (dreamName == "Dream_shoppingAlt")
//		{
//			return SaveManager.save.spAlt;
//		}
//		if (dreamName == "Dream_tech")
//		{
//			return SaveManager.save.tc;
//		}
//		if (dreamName == "Dream_techAlt")
//		{
//			return SaveManager.save.tcAlt;
//		}
//		if (dreamName == "Dream_followers")
//		{
//			return SaveManager.save.fw;
//		}
//		if (dreamName == "Dream_followersAlt")
//		{
//			return SaveManager.save.fwAlt;
//		}
//		if (dreamName == "Dream_indulgence")
//		{
//			return SaveManager.save.id;
//		}
//		if (dreamName == "Dream_indulgenceAlt")
//		{
//			return SaveManager.save.idAlt;
//		}
//		if (dreamName == "Dream_exercise")
//		{
//			return SaveManager.save.ec;
//		}
//		if (dreamName == "Dream_exerciseAlt")
//		{
//			return SaveManager.save.ecAlt;
//		}
//		if (dreamName == "Dream_career")
//		{
//			return SaveManager.save.cr;
//		}
//		if (dreamName == "Dream_careerAlt")
//		{
//			return SaveManager.save.crAlt;
//		}
//		if (dreamName == "Dream_money")
//		{
//			return SaveManager.save.sv;
//		}
//		if (dreamName == "Dream_moneyAlt")
//		{
//			return SaveManager.save.svAlt;
//		}
//		if (dreamName == "Dream_dating")
//		{
//			return SaveManager.save.dt;
//		}
//		if (dreamName == "Dream_datingAlt")
//		{
//			return SaveManager.save.dtAlt;
//		}
//		if (dreamName == "Dream_pressure")
//		{
//			return SaveManager.save.pr;
//		}
//		if (dreamName == "Dream_pressureAlt")
//		{
//			return SaveManager.save.prAlt;
//		}
//		if (dreamName == "Dream_time")
//		{
//			return SaveManager.save.ft;
//		}
//		if (dreamName == "Dream_timeAlt")
//		{
//			return SaveManager.save.ftAlt;
//		}
//		if (dreamName == "Dream_mind")
//		{
//			return SaveManager.save.mn;
//		}
//		if (dreamName == "Dream_mindAlt")
//		{
//			return SaveManager.save.mnAlt;
//		}
//		if (dreamName == "Dream_space")
//		{
//			return SaveManager.save.wr;
//		}
//		if (dreamName == "Dream_spaceAlt")
//		{
//			return SaveManager.save.wrAlt;
//		}
//		if (dreamName == "Dream_nature")
//		{
//			return SaveManager.save.nr;
//		}
//		if (dreamName == "Dream_natureAlt")
//		{
//			return SaveManager.save.nrAlt;
//		}
//		if (dreamName == "Dream_meditation")
//		{
//			return SaveManager.save.md;
//		}
//		if (dreamName == "Dream_meditationAlt")
//		{
//			return SaveManager.save.mdAlt;
//		}
//		if (dreamName == "Dream_stress")
//		{
//			return SaveManager.save.fr;
//		}
//		if (dreamName == "Dream_stressAlt")
//		{
//			return SaveManager.save.frAlt;
//		}
//		if (dreamName == "Dream_desires")
//		{
//			return SaveManager.save.me;
//		}
//		if (dreamName == "Dream_desiresAlt")
//		{
//			return SaveManager.save.meAlt;
//		}
//		if (dreamName == "Dream_past")
//		{
//			return SaveManager.save.ps;
//		}
//		if (dreamName == "Dream_pastAlt")
//		{
//			return SaveManager.save.psAlt;
//		}
//		if (dreamName == "Dream_future")
//		{
//			return SaveManager.save.fu;
//		}
//		if (dreamName == "Dream_futureAlt")
//		{
//			return SaveManager.save.fuAlt;
//		}
//		if (dreamName == "Dream_anxiety")
//		{
//			return SaveManager.save.ax;
//		}
//		if (dreamName == "Dream_anxietyAlt")
//		{
//			return SaveManager.save.axAlt;
//		}
//		if (dreamName == "Dream_final")
//		{
//			return SaveManager.save.fn;
//		}
//		if (dreamName == "Dream_finalAlt")
//		{
//			return SaveManager.save.fnAlt;
//		}
//		return 0;
//	}

//	// Token: 0x06000670 RID: 1648 RVA: 0x00035B9E File Offset: 0x00033D9E
//	public int GetMinutesPlayed()
//	{
//		return Mathf.RoundToInt(SaveManager.save.min);
//	}

//	// Token: 0x06000671 RID: 1649 RVA: 0x00035BAF File Offset: 0x00033DAF
//	public float GetHoursPlayed()
//	{
//		return Mathf.Round(SaveManager.save.min / 60f * 100f) / 100f;
//	}

//	// Token: 0x06000672 RID: 1650 RVA: 0x00035BD2 File Offset: 0x00033DD2
//	public bool CheckAllStarsAchievement()
//	{
//		return this.GetTotalEarnedStars() == 63;
//	}

//	// Token: 0x06000673 RID: 1651 RVA: 0x00035BE1 File Offset: 0x00033DE1
//	public bool CheckAllRingsAchievement()
//	{
//		return this.GetTotalEarnedRings() == 63;
//	}

//	// Token: 0x06000674 RID: 1652 RVA: 0x00035BF0 File Offset: 0x00033DF0
//	public bool CheckPerfectionistAchievement()
//	{
//		return SaveManager.save.fd + SaveManager.save.sp + SaveManager.save.tc + SaveManager.save.fw + SaveManager.save.id + SaveManager.save.ec + SaveManager.save.cr + SaveManager.save.sv + SaveManager.save.dt + SaveManager.save.pr + SaveManager.save.ft + SaveManager.save.mn + SaveManager.save.wr + SaveManager.save.nr + SaveManager.save.md + SaveManager.save.fr + SaveManager.save.me + SaveManager.save.ps + SaveManager.save.fu + SaveManager.save.ax + SaveManager.save.fn + SaveManager.save.fdAlt + SaveManager.save.spAlt + SaveManager.save.tcAlt + SaveManager.save.fwAlt + SaveManager.save.idAlt + SaveManager.save.ecAlt + SaveManager.save.crAlt + SaveManager.save.svAlt + SaveManager.save.dtAlt + SaveManager.save.prAlt + SaveManager.save.ftAlt + SaveManager.save.mnAlt + SaveManager.save.wrAlt + SaveManager.save.nrAlt + SaveManager.save.mdAlt + SaveManager.save.frAlt + SaveManager.save.meAlt + SaveManager.save.psAlt + SaveManager.save.fuAlt + SaveManager.save.axAlt + SaveManager.save.fnAlt == 168;
//	}

//	// Token: 0x040002AE RID: 686
//	public static SaveManager mgr;

//	// Token: 0x040002AF RID: 687
//	private static save save;

//	// Token: 0x040002B0 RID: 688
//	private static float timeSaved;
//}
