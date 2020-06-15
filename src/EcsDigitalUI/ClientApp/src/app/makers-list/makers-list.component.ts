import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-makers-list',
  templateUrl: './makers-list.component.html'
})
export class MakersDataComponent {
  public makers: IMaker[];
  public newMakerName: string;
  public newMakerNameExists: boolean;
  public http: HttpClient;
  constructor(http: HttpClient) {
    this.http = http;
    this.refreshMakers();
  }

  private refreshMakers() {
    this.http.get<IMaker[]>('http://localhost:58599/api/' + 'maker').subscribe(result => {
      this.makers = result;
    }, error => console.error(error));
  }
  public async addNewMaker() {
    let newMakerName = this.newMakerName;
    (await this.http.post('http://localhost:58599/api/' + 'maker', { name: newMakerName }))
      .subscribe({
        next: async () => {
          this.newMakerName = null;
          while (!this.makers.find(p => p.name === newMakerName)) {
            await new Promise(r => setTimeout(r, 800));
            this.refreshMakers();
          }
        },
        error: error => console.error(error)
      });
  }

  public validateMakerExists() {
    if (this.makers.find(p => p.name === this.newMakerName)) {
      this.newMakerNameExists = true;
    } else {
      this.newMakerNameExists = false;
    }
  }

}

interface IMaker {
  id: number;
  name: string;
}

