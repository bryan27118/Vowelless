function Game::displayBattleGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayHealthBar(Player.Health,"-10 32");
	Game.displayTime();
	Game.displayRound();
	Game.displayBackPanel("GameAssets:panelbeige");
	Canvas.pushDialog(GameGui);
}

function Game::startNewRound()
{
	if(stricmp(Game.Mode,"Battle") == 0)
	{
		if(Player.Health <= 0)
		{
			Canvas.popDialog(GameGui);
			Game.displayLoseScreen();
			return;
		}
		else if(AI.Health <= 0)
		{
			Canvas.popDialog(GameGui);
			Game.displayWinScreen();
			return;
		}
	
		if(Game.Round == 3)	//Determine Winner
		{
			Canvas.popDialog(GameGui);
			
			if(Player.Health > AI.Health)
			{
				Game.displayWinScreen();
			}
			else if(Player.Health < AI.Health)
			{
				Game.displayLoseScreen();
			}
			else
			{
				Game.displayLoseScreen();
			}
		}
		else
		{
			Game.Round++;
			Game.Time = 30;
			Player.Damage = 0;
			AI.Damage = 0;
			Player.CurrentWord++;
			AI.CurrentWord++;
			Answer.setText("");
			Game.displayBattleGame();
			Game.schedule(2000,"incrementTime");
			AI.schedule(2000,"readWord");
		}
	}
}

function Game::displayRound()
{
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "-38 33";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = "Round:" SPC Game.Round @ "/3";
	};  
		
	MainScene.add(%obj);
}

function Game::displayBattleStats()
{
	MainScene.clear();

	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "-30 30";
		FontSize = "8 8";
		Layer = 2;
		TextAlignment = "Center";
		Text = "You";
	};
	
	MainScene.add(%obj);
	
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "30 30";
		FontSize = "8 8";
		Layer = 2;
		TextAlignment = "Center";
		Text = "AI";
	};  
		
	MainScene.add(%obj);
	
	Game.displayHealthBar(Player.Health,"-40 10");
	Game.displayHealthBar(AI.Health,"20 10");
	
	%playerDamage = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "-30 0";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = "Damage:" SPC Player.Damage;
	};  
		
	MainScene.add(%playerDamage);
}

function Game::endBattleGame(%winner)
{

}

function Game::playImpactSound(%this)
{
	setRandomSeed(getRealTime());
	
	%roll = getRandom(0,2);
	
	if(%roll == 0)
	{
		alxPlay("GameAssets:impact1");
	}
	else if(%roll == 1)
	{
		alxPlay("GameAssets:impact2");
	}
	else if(%roll == 2)
	{
		alxPlay("GameAssets:impact3");
	}
}

function Game::playHitSound(%this, %damage)
{
	if(mAbs(%damage) <= 5)
	{
		alxPlay("GameAssets:Weakhit");
	}
	else if(mAbs(%damage) <= 10)
	{
		alxPlay("GameAssets:Mediumhit");
	}
	else if(mAbs(%damage) <= 15)
	{
		alxPlay("GameAssets:Stronghit");
	}
}

function Game::startBattle()
{
	Canvas.popDialog(GameGui);
	Game.displayBattleStats();
	Player.schedule(1000,"attackAI",-Player.Damage/3);
	Player.schedule(2000,"attackAI",-Player.Damage/3);
	Player.schedule(3000,"attackAI",-Player.Damage/3);
	AI.schedule(4000,"attackPlayer",-AI.Damage/3);
	AI.schedule(5000,"attackPlayer",-AI.Damage/3);
	AI.schedule(6000,"attackPlayer",-AI.Damage/3);
	Game.schedule(7000,"startNewRound");
}