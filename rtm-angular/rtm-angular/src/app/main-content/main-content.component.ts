import { Component, OnInit, Output } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ChatService } from '../chat.service';
import { FormGroup, FormControl, FormBuilder, FormArray } from '@angular/forms';
import { Workspace } from '../Workspace';
import { Channel } from '../Channel';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../User';
import { Message } from '../Message';
import { Observable } from 'rxjs';
//import { EventEmitter } from 'protractor';

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
  channelId: string;
  workspaceName: string;
  channel: Channel;
  userid: string;
  allUsers=[];
  //username: string;
  emailId: string;
  channelArray: Channel[];
  channelName: string;
currentuser:User;
directMessageChat: Channel[] = [];


  public joinChannel(channelId: string): void {
    this._hubConnection
      .invoke('JoinChannel', channelId)
      .catch(err => console.error(err));
    console.log("in join Channel");
  }


  public sendMessageInChannel(): void {
    this._hubConnection
      .invoke('SendMessageInChannel', this.emailId, this.channelmessage, this.channelId)
      .then(() => this.channelmessage = '')
      .catch(err => console.error(err));
  }


  orderObj;
  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      this.orderObj = { ...params.keys, ...params };
      console.log(this.orderObj["params"]["email"]);
    });
    this.emailId = this.orderObj["params"]["email"];
    this.workspaceName = this.orderObj["params"]["workspace"];

    this.chatservice.setEmailAndWorkspace(this.emailId, this.workspaceName);

    //Get list of users in the workspace
    this.getListOfUsersInWorkspace();
    console.log(this.allUsers);

    //this.currentuser = this.allUsers.find(x => x.emailId == this.emailId);
    //console.log(currentUser);
    this.chatservice.setListOfUsers(this.allUsers);

    //Get list of channels in workspace
    this.ListAllChannels();
  }

  getListOfUsersInWorkspace() {
    console.log("get list of users in workspace");
    console.log(this.workspaceName);
    this.chatservice.getAllUsersInWorkspace(this.workspaceName)
      .subscribe(s => {
        this.allUsers = s;
        this.currentuser = s.find(x => x.emailId == this.emailId);
      });
      console.log(this.allUsers);
  }


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private chatservice: ChatService,
    private fb: FormBuilder) {
    this.channelArray = new Array<Channel>();
    this._hubConnection = new HubConnectionBuilder()
      .withUrl('http://172.23.238.230:5004/chat')
      .build();

    this._hubConnection.on('JoinChannel', (channelId: string) => {
      console.log("in joinchannel method" + channelId);
    });


    this._hubConnection.on('SendMessageInChannel', (username: string, receivedMessage: string) => {
      console.log("in sendtochannel method");
      const text = `${username}: ${receivedMessage}`;
      this.channelmessages.push(text);
      if(username != this.emailId){
      this.notify();
      }
    });
    this._hubConnection
      .start()
      .then(() => { console.log('Connection started!') })
      .catch(err => console.log('Error while establishing connection :('));

    console.log(this._hubConnection);
  }


  startChannelCommunication(): void {
    //console.log("in startChannelCommunication ");
    this.chatservice.getChannelIdByWorkspaceName(this.workspaceName)
      .subscribe(s => this.channelId = s.channelId);
  }

  ListAllChannels(){
    console.log("in list channel function");
    this.chatservice.getUserChannels(this.emailId, this.workspaceName)
      .subscribe(s => {
        console.log(s);
        this.channelArray = s;
        for(let channel of s){
          let c = channel as Channel;
          //console.log(this.currentuser);
          var index = c.channelName.indexOf(this.currentuser.userId);
          console.log(index);
          if(c.channelName.indexOf(this.currentuser.userId) != -1){
            this.directMessageChat.push(c);
            console.log(this.channelArray.indexOf(c))
            this.channelArray.splice(this.channelArray.indexOf(c), 1);
          }

        }
        console.log(this.directMessageChat);
      });
    console.log(this.channelArray);

   }
  getChannelName(channel: Channel) {
    console.log("get channel id");
    console.log(channel.channelName);
    this.channelName = channel.channelName;
    this.channelId = channel.channelId;
    console.log(this.channelName);
    this.channelmessages = [];
    this.joinChannel(channel.channelId);
  }

  // @Output()
  // currentUserEmail = new EventEmitter();

  Channel() {
    this.chatservice.setListOfUsers(this.allUsers);
    this.router.navigate(['addChannel']);
    //this.currentUserEmail.emit(this.emailId);
  }

  public notify() {
    let audio = new Audio();
    audio.src = "../assets/sounds/unconvinced.mp3";
    audio.load();
    audio.play();
  }

}
