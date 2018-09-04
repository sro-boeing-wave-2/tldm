import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ChatService } from '../app/chat.service';
import { FormGroup, FormControl, FormBuilder, FormArray } from '@angular/forms';
import { Workspace } from './Workspace';
import { Channel } from './Channel';
import { User } from './User';
import { Message } from './Message';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  public _hubConnection: HubConnection;
  // nick = '';
  // message = '';
  // messages: string[] = [];
  // id = '';
  // ids: string[] = [];
  // nick1 = '';
  // nick1s: string[] = [];
  // grpmsg = '';
  // grpmsgs: string[] = [];
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


  // public sendMessage(): void {
  //   this._hubConnection
  //     .invoke('sendToAll', this.nick, this.message)
  //     .then(() => this.message = '')
  //     .catch(err => console.error(err));
  // }

  // public getid(): void {
  //   this._hubConnection
  //     .invoke('printId', this.nick)
  //     .catch(err => console.error(err));
  // }

  // public join(): void {
  //   this._hubConnection
  //     .invoke('JoinGroup', this.nick)
  //     .catch(err => console.error(err));
  //   console.log("in join method" + this.nick);
  // }
  public joinChannel(): void {
    this._hubConnection
      .invoke('JoinChannel', this.channelId)
      .catch(err => console.error(err));
    console.log("in join Channel");
  }

  // public sendMessageingrp(): void {
  //   this._hubConnection
  //     .invoke('SendMessageToGroups', this.nick, this.grpmsg)
  //     .then(() => this.grpmsg = '')
  //     .catch(err => console.error(err));
  // }
  //sending message in channel
  public sendMessageInChannel(): void {
    this._hubConnection
      .invoke('SendMessageInChannel', this.emailId, this.channelmessage, this.channelId)
      .then(() => this.channelmessage = '')
      .catch(err => console.error(err));
  }

  // public leave(): void {
  //   this._hubConnection
  //     .invoke('LeaveGroup', this.nick)
  //     .catch(err => console.error(err));
  //   console.log("in leave method" + this.nick);
  // }

  ngOnInit() {
  }

  // WorkSpaceForm = this.fb.group({
  //   workspaceId: [''],
  //   workspaceName: [''],
  //   channels: this.fb.array([])
  // });
  // UserForm = this.fb.group({
  //   userId: [''],
  //   firstName: [''],
  //   lastName: [''],
  //   userName: [''],
  //   designation: [''],
  //   workspaceName: ['']
  // });
  // MessageForm = this.fb.group({
  //   messageId: [''],
  //   messageBody: [''],
  //   timeStamp: [''],
  //   isStarred: [''],
  //   sender: this.UserForm
  // });
  // ChannelForm = this.fb.group({
  //   channelId: [''],
  //   channelName: [''],
  //   users: this.fb.array([]),
  //   messages: this.fb.array([]),
  //   admin: this.UserForm
  // });
  // get Channels() {
  //   return this.WorkSpaceForm.get('channels') as FormArray;
  // }
  // get Users() {
  //   return this.ChannelForm.get('users') as FormArray;
  // }
  // get Messages() {
  //   return this.ChannelForm.get('messages') as FormArray;
  // }
  // addChannels() {
  //   this.Channels.push(this.ChannelForm);
  // }
  // addUsers() {
  //   this.Channels.push(this.UserForm);
  // }
  // addMessages() {
  //   this.Channels.push(this.MessageForm);
  // }

  // WorkspaceToCreate: Workspace;

  constructor(
    private chatservice: ChatService,
    private fb: FormBuilder) {
    //initializing the workspace to be created
    //this.WorkspaceToCreate = new Workspace();
    this.channelArray = new Array<Channel>();
    //this.nick = window.prompt('Your name:', '');
    this._hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/chat')
      .build();
    // this._hubConnection.on('sendToAll', (nick: string, receivedMessage: string) => {
    //   console.log("in sendtoall method");
    //   const text = `${nick}: ${receivedMessage}`;
    //   this.messages.push(text);
    // });
    // this._hubConnection.on('printId', (id: string) => {
    //   console.log("in printid method");
    //   const text1 = `${id}`;
    //   console.log(text1);
    //   this.ids.push(text1);
    // });
    // this._hubConnection.on('JoinGroup', (nick1: string) => {
    //   console.log("in joingroup method" + nick1);
    //   const text1 = `${nick1}`;
    //   console.log(text1);
    //   this.nick1s.push(text1);
    // });
    this._hubConnection.on('JoinChannel', (channelId: string) => {
      console.log("in joingroup method" + channelId);
    });


    // this._hubConnection.on('LeaveGroup', (nick: string) => {
    //   console.log("in LeaveGroup method" + nick);
    //   const text1 = `${nick}`;
    //   console.log(text1);
    //   this.nick1s.push(text1);
    // });
    // this._hubConnection.on('SendMessageToGroups', (nick: string, receivedMessage: string) => {
    //   console.log("in sendtogroup method");
    //   const text = `${nick}: ${receivedMessage}`;
    //   this.grpmsgs.push(text);
    // });
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

  // createworkspace() {
  //   console.log(this.WorkSpaceForm.value);
  //   this.chatservice.CreateWorkspace(this.WorkSpaceForm.value)
  //     .subscribe(s => console.log(s));
  // }

  // addusertoworkspace() {
  //   console.log("i am in addusertoworkspace");
  //   this.chatservice.addUserToWorkSpace(this.UserForm.value)
  //     .subscribe(s => console.log(s));
  // }
  startChannelCommunication(): void {
    console.log("in startChannelCommunication ");
    this.chatservice.getChannelIdByWorkspaceName(this.workspaceName)
      .subscribe(s => this.channelId = s.channelId);
  }
  // getUserWorkspace() {
  //   console.log("getting user workspace");
  //   this.chatservice.getUserById(this.userid)
  //   .subscribe(s => {this.workspaceName = s.;
  //                    this.username = s.userName;
  //                    this.chatservice.getChannelIdByWorkspaceName(this.workspaceName)
  //                    .subscribe(s => {this.channelId = s.channelId;
  //                                     this.joinChannel();  }); } );
  // }
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
  }



}
