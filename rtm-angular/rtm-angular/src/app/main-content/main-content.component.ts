import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ChatService } from '../chat.service';
import { FormGroup, FormControl, FormBuilder, FormArray } from '@angular/forms';
import { Workspace } from '../Workspace';
import { Channel } from '../Channel';
import { ActivatedRoute,Router } from '@angular/router';
import { User } from '../User';
import { Message } from '../Message';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.css']
})
export class MainContentComponent implements OnInit {

  public _hubConnection: HubConnection;
  channelmessage = '';
  channelmessages = [];
  workspacenamebyuser = '';
  newusertoworkspace = '';
  channelId :string;
  workspaceName: string;
  channel: Channel;
  userid:string;
  username:string;
  emailId:string;
  channelArray:Channel[];
  channelName:string;
  // public sampleChannel={
  //   "channelId":"";
  // "channelName":"";
  // "users":User[];
  // "admin":User;
  // messages:Message[];
  // workspaceId:string;
  // }



  public joinChannel(): void {
    this._hubConnection
      .invoke('JoinChannel', this.channelId)
      .catch(err => console.error(err));
    console.log("in join Channel");
  }


  public sendMessageInChannel(): void {
    this._hubConnection
      .invoke('SendMessageInChannel', this.emailId, this.channelmessage, this.channelId)
      .then(() => this.channelmessage = '')
      .catch(err => console.error(err));
  }


  ngOnInit() {
  }


  constructor(
    private router:Router,
    private chatservice: ChatService,
    private fb: FormBuilder) {
    this.channelArray = new Array<Channel>();
    this._hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/chat')
      .build();

    this._hubConnection.on('JoinChannel', (channelId: string) => {
      console.log("in joingroup method" + channelId);
    });



    this._hubConnection.on('SendMessageInChannel', (username: string, receivedMessage: string) => {
      console.log("in sendtochannel method");
      const text = `${username}: ${receivedMessage}`;
      this.channelmessages.push(text);
    });
    this._hubConnection
      .start()
      .then(() => { console.log('Connection started!') })
      .catch(err => console.log('Error while establishing connection :('));

    console.log(this._hubConnection);
  }


  startChannelCommunication(): void {
    console.log("in startChannelCommunication ");
    this.chatservice.getChannelIdByWorkspaceName(this.workspaceName)
      .subscribe(s => this.channelId = s.channelId);
  }

  ListAllChannels(){
    console.log("in list channel function");
    this.chatservice.getUserChannels(this.emailId,this.workspaceName)
    .subscribe(s => {
      console.log(s);
      this.channelArray = s;});
  }
  getChannelName(channel: Channel){
    console.log("get channel id");
    console.log(channel.channelName);
    this.channelName = channel.channelName;
    this.channelId = channel.channelId;
    console.log(this.channelName);
    this.channelmessages = [];
  }

Channel()
{
this.router.navigate(['addChannel']);

}


}
