import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ConfigService } from '../app-config/config.service';
import { GameDataService } from './game-data.service';
import { Game } from './game.model';

@Injectable()
export class GameApiService {

    constructor(private configService: ConfigService, private httpClient: HttpClient, private gameDataService: GameDataService) { }

    public async setGameScores(gameId: string): Promise<void> {
        this.gameDataService.setNewGameData(null);

        if (gameId != null) {
            let game = await this.httpClient.get<Game>(`${this.configService.config.apiUrl}api/game/scores/${gameId}`).toPromise();
            this.gameDataService.setNewGameData(game);
        }
    }
}
