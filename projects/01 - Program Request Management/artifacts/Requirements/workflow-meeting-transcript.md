00:00:02
We have a, I think they have the new version. What is it called? This one? It's called, called the Plaud. Plaud, recording AI or something. P-L-A-U-D.

00:00:32
Oh, so this was for the, this one, the link you sent to me for the review, I couldn't access. Okay, maybe they just committed something to the dev, but not working. I will take a look.

00:01:04
So this was Warren's, so it's kind of like similar to what I have in the document already, just a few changes, and this was generated because of the status, because I wanted to know the status, so I was talking to Warren to find out what this looked like. So, so far from this, before I show you, I want to talk about, so this was, from that.

00:01:44
discussion, I was able to narrow down the status, open, so this is additional information I just added to the, based on that workflow. From the other workflow, right? Yes. So, when the ticket is created. If not assigned, they start to be open. When it's assigned, then ticket is assigned. Status should be acknowledged when the engineer acknowledges the assignment,

00:02:21
unacknowledged if the engineer hasn't accepted the assignment. Part of the discussion we had with them, with Jason, was they wanted it to be able to have two stages. When the engineer acknowledges, they've acknowledged, but they also want the status to change when the engineer starts working on the program. So they want like a button they can toggle to say it is in progress.

00:02:54
Okay, the question here is about the process. So Jason, as an engineer, you have a request, and we have the data, the new process, the email, send it to the email box for the request, it will generate a request automatically, it's open,

00:03:25
and it will find out that there's a state diagram, import that state diagram, and get the data imported. That data is a kind of drafting status, so the data is not taken, okay? And before engineers, start doing that, we need one step, they need to verify this data against the state diagram.

00:03:57
Basically, we provide them with tools that Julia built that will be in that format. They will have the data tree on the left side, the state diagram in the middle, the data on the right side. They click that and it will show where this data is and what the value is. If the value is not correct, being parsed automatically, they can fix it. Then, they will get the right data. As a process, I think, they acknowledge it, they pick it.

00:04:32
up, but the first step for their working, they need to do the data verification to validate that data. Otherwise, they can't move on to use that data. They can't see that data. I think that can be triggered. They open that data. a data auditor, and then to save the data, we can see there is data working on it. So we can consider... That could be our in-progress trigger.

00:05:05
Yeah. Okay. Right there. That's what I'm thinking, yeah. Okay. That should be the trigger. So I actually like that. So the in-progress, right there, we need to add in our validation. Yeah. Okay. Before the, after acknowledge, and then the staff, the engineer, validate the sleep diagram, imported data, yeah, to start their job line.

00:05:38
I like that. Validation help. Yes. Thank you. Notice is this the same drop-down like in our tool? Yeah, this is the same column. Yes So so if it's the same column, yes, we don't need on that college. Okay, the reason I say.

00:06:15
It's it's either gonna be open or a sign Yeah, when I acknowledge it I acknowledge it when do I say unacknowledged like who says? No, so it was supposed to be when you know, Because we change, Warren wants that assigned ticket to go to all the teams, everyone in the team, not just the person that is assigned to. Now, if the person that is assigned to now acknowledges it, like there may be a button in the email and say it's accepted, the status changes to acknowledged.

00:06:57
But if they haven't accepted that assignment, it will remain unacknowledged so that Jeremy can see that the person that is assigned to hasn't acknowledged. So what do I need the assigned button for? The assign is to show that it has been assigned. But the second that I push that button, it's been assigned, so then it flips to unacknowledged?

00:07:27
The person that is being assigned to, because now the email is going to all the team, not just the person, so immediately they accept that it has been assigned, it automatically shows they've acknowledged the assignment. Okay, let's say that I don't do that. It stays on and off, or it goes through, I guess. When do I do that? That's my curiosity. Okay, so maybe unacknowledged can go, but I think acknowledged is important.

00:08:02
I think it's not called acknowledged. Basically, you assign to an engineer, the engineer can accept it or decline. Yeah. Right? They can accept it, I will comment. But I'm so busy, I will decline. And decline will go back to open, but we send a message to the manager or other people saying, I'm too busy, I can't, they can add a comment or whatever. Say, one, the senator will say, Jeremy assigned to Henry, but this is not Henry's client.

00:08:39
Henry just declined it, and Reynolds, this is Jesus' client. Yeah, that I like better. I just, we have one too many in here. Yeah. Because they overlap. It's good that you write it down like this, so I like this, because then we can walk it through.

00:09:09
Do we, are you okay with this? yeah we're gonna have to talk with engineering about whether it's acknowledged or not whatever whatever the wording they want yeah but we either say yes or we say no what happens when we do nothing yeah when we do nothing I.

00:09:41
think that's when it should be unacknowledged okay what's the warring warring is the I think warring here is I'm seeing his idea is like a sun one say it's a sun is high in there yes waiting for somebody to accept it yes but there will be you know a timing for like, If that guy didn't, you know, accept it, this one is just overdue, and he wants something.

00:10:24
and say, yeah, it's time over. Then somebody will need to handle it, that will be automation to, based on the, you know, the time frame, say, you know, how long it should be accepted, it's urgent, how long it should be accepted, you know, it's not urgent, how long it should be accepted. If it's not accepted, what should we do? I think that may be the idea, I can understand.

00:10:56
That is the idea, but I'm just, where I struggle with is... It's assigned and it's unacknowledged at the same time. No, it can't be at the same time. It is, because I assign it. The second I assign it, it's assigned. Yes. But nobody's accepted it, so it's unacknowledged.

00:11:29
Okay, yes. And it's assigned and unacknowledged until somebody accepts it. Yeah. And that's where I struggle, is those two in my head are the same. Okay, but Adam is also correct that how do I decline this to create the loop so that it can be reassigned?

00:12:00
Reassigned, yes. But also, if we send it to the whole group, But I assign Henry, Henry doesn't do anything. Any of the other group members accept it? I think here is the same as our flat service logic, right? Somebody needs to triage the ticket.

00:12:33
Yeah, and our triage is going to be automated for most of our clients based on our client list, right? That's our triage. Here is the, okay, forget about any automation. So the first thing we need to do is see how people do things. And then, say, we auto-triage is to just say the people, the road to triage the ticket. We just use some, shift his workload to automation. But it doesn't mean it's still on his behalf.

00:13:06
Yes. Yeah. So it's, let's say, Jeremy will charge the tickets, and here we have the automation. He said, okay, AI, help me to do it. And then, but if anything come back, he still need to do that. And we still need to build the coverage for Jeremy, if his role is, he's off, and somebody.

00:13:36
has to be assigned to this role, too. Yeah. So you wanted to say something? The management of that, currently in teams for blend approval, we can add in people.

00:14:07
very, very easily. Yeah, just to the Google, right? Yeah, and so the movie in my head has something similar to that to control this, right? Jeremy is the assigner. I can add Warren, I can add whoever I want. I can add me if I want to, and then I can assign required. So to me, and then we send it to the engineering group and the same thing, we have a list of people.

00:14:39
and in a perfect world, we're bamboo. We pull out the engineering people from there. When you hire somebody, we don't have the client control, how it's assigned for the automation part. Yeah, that's our security management. We are going to clean up the data, and that will be, we will define the rules, and that will be integrated with the security group, the ADC group, and even Bambu.

00:15:17
And say, we just need to move people to, you know, to this role, and everything will be aligned. And then, I think, for this, what we need to think about, okay, so you sign the ticket. That works really good when somebody accepts it. But we need to be cognizant of the fact that, okay, it sits there for a while. What status is that? I understand it's unacknowledged, but it's still a sign. So how do we control that?

00:16:02
we decline it because I'm too busy on something. I don't know how to do it in that or decline. So we can do this that if unacknowledged if the tickets will be assigned but if not.

00:16:33
acknowledged after like four hours or five hours it goes to unacknowledged. I remember there are rules for different priority right? Yeah we'll have to follow their rules based on them. But let's think about this decline for a second. Because I'm going to send this to five people.

00:17:05
So about this decline, I think the reassign button is going to work. Because even in the diagram, Warren wants to be able to reassign this to maybe the on-call guy. Maybe you're getting it at the end of the day, and you're unable to work on it, and you reassign it to on-call guy. So it might not be declined, it might be reassigned.

00:17:36
Sure. But let's think about decline for a second. So I send it to five people. Assign to one of them. Yes. And the other four decline, and the one it's assigned to does nothing. What happens? I'm thinking, you know, we talk about the role of the ticket. So, if it declines, let's just say it will go back to manual sign process, it will be, this ticket will be assigned to that role, to Jeremy, and Jeremy can handle it, or Warren, you know, Carl Jeremy, Warren will do that. And Warren can, it's assigned to him, he can really assign to a lot of people.

00:18:30
Just, as I said, we need to reassign, even Henry can reassign to a lot of people, and, you know, directly, if they have some, you know, conversation, yeah. Yeah, that's my concern a little bit, though, when I assign it, and they want it to go to the whole group, that's where my concern comes from. No, no, you reassign it, it won't go to the whole group, it's going to...

00:19:01
you know either go back you decline it's go back to the the people who yeah to germany if you reassign you can reassign to anybody and only that guy will receive you and if germany wants to get the notification this one being reassigned and he will get the notification he'll he let's just put it in that he will want that yeah my concern is on the assign it going to the whole group.

00:19:34
why it's going to a song is just between germany and the harry you know this this comment was they wanted to hold i think i think they reassigned no just the assign yeah they assigned because when we built this even in this document that assigned was supposed to go to just the person, But then Jeremy said to go to the whole group. This is not a critical thing, this is just a notification to the whole group.

00:20:12
So when we say that we are going to notify, it just changes the whole group, the assigning. But, okay, but can we control, let's say that I assign it to Henry and everybody gets an email, but I have an acknowledge, decline, or reassign button. We don't have an acknowledge button then, we just accept or decline, just two buttons.

00:20:45
Accept or decline, how do I reassign? Under reassign button, yeah. Okay, so I have three buttons. Does everybody see the three buttons? No, it's only, see, it is assigned to Henry, only Henry can reassign it. He's the only one that gets the buttons? Yeah. Okay, if that's our control, then I'm good. Yeah, and on the web interface, and, you know, it's the same thing, and Jeremy can see it,

00:21:15
can see the reassign button. He assigns to Henry, he realizes, oh, I should assign to Jordan, and he does reassign to the center to Jordan. I think the other people in the group, that's where the person is assigned to. Just get a notification. Just get a notification field without the buttons. That was my concern. Yeah. That also complicates the buildup. Yes.

00:21:48
Then I'm good. So the acknowledges really say accepted, yeah. The unacknowledged is just reassigned or declined, right? Yeah. It's declined or reassigned. Declined will go back to open. Yes. And Jeremy will get a notification he declined.

00:22:21
Actually, declined can go back to Jeremy directly, or assigned to Jeremy, and Jeremy reassigned, yeah. Okay, so Jeremy will now... Do you want a new book? No, I'm done. I actually forgot my book at home, so I had to bring this one. I took that little book. I'll get you a new book today. Okay, thanks. I won't mind. That would be nice. So I can keep one in the office.

00:22:51
So the reassign goes back to... I'm sorry, it goes back, but I have a question. Now, if, for example, if, okay, yeah, we already answered that question. The person assigned can reassign the person in general.

00:23:28
So the in-progress comes in when we use any of the validation steps, the stick diagram. When it's sent for review, this is where review is sent to the group. They send the review to the group. It should trigger that. Yeah, it's just about incentive for review.

00:23:59
If I come back to approve, so awaiting review, do we want to have this if the reviewer accepted to review? I don't understand the difference between this. Okay, so what happens here is a member sent for review is sent to the group. If you see, I canceled this awaiting approval for approval because approval is just Jeremy or one person.

00:24:31
But the difference here is the review is sent to the group. It's not sent to one person. So somebody in the group has to accept, even though you've talked to the person or something. Do you understand? Another button to click that I accept. because review goes to the table. I understand. Yeah, I'm thinking that is it waiting for review, and you click send for review.

00:25:02
And send to review will be a button. It's not a status, because your status will be waiting for approval, waiting for review. So send for review is not a status. It's just a button. You click that button, and then it's a- Automatically, it just goes to a waiting review. So we shouldn't have send for-

00:25:33
This is a tool, the send for review, and a waiting for review, they are same thing. You click send to review, and it will become, the set of a waiting review. And then you will have another one you want to see. No, no.

00:26:25
no, no, Jeremy's words, so... Okay, waiting review, but a waiting review is a review review not started, so then next time review started.

00:26:55
Review completed. I think Graham might be a lawyer. Yeah, that's what he's thinking, right? So then you should have that reviewing that means review started, but how we trigger the review. Yeah, do we want to have review started, or we just skip the review? But they accept it, but they just accept it, but they don't review it, and they will review it another time.

00:27:25
Yeah, it's, you know, we are getting to a really granular level, you know. I'm okay with that, it's just adding some more covers, but that's okay, yeah. Okay. So, awaiting review is good, review is good. Jason, is that okay? Let's keep the word. I understand. Awaiting review, that it is accepted, and it's not.

00:27:57
being reviewed yet. Review not complete yet, right? Okay, yeah. Awaiting review. But why should they have the reviewed, because the decision is, they will, so review, okay, the review will come to two results. One is, you know, no problem. Another, you.

00:28:29
have problem, give back, right? So, where, where there's, you know. there should be two say after the review they say it's good it's not good okay and what should it's good it'll come to uh send for approval no review and approval separate yeah yeah it's, does it need reviewer approval red and black are approved green and yellow are reviewed okay and.

00:29:01
and then it comes back okay so yeah so still is it is another kind of approval but it's called review that's correct yeah so but still it's reviewed okay so we have we need to have reviewed approved or reviewed rejected i'm trying to think of a word in my head i get what you mean they reviewed but the review is not good what was the review pass.

00:29:34
you, It's passed. Yeah, it's review passed, review failed. Okay. So we have to, we should have to review passed? Yeah. Just approve the decline, right? Review passed. And review failed. Where review failed? You know, go back to the, reassign back to the engineer to which status is.

00:30:08
To be assigned? No. Assigned is. Okay. Yeah, and the assignee will be, no, we are not going to change, we will have the reviewer and the approver.

00:30:43
We are not going to change the assignee. It's currently over there, he is the Indian European Assignment Board on it, and we have, if it's going to review process, we will have a reviewer. And if it's going to approval, we will have an approver. Okay, so this is right. So now to the major thing, which is approval.

00:31:18
So we had, just remember the discussion we had on approval, so approval is going to be the one that is tricky. So they have three approvals, approval method of types, what should I call it, they said conditional, technical, and commercial approval. It's the same, it's the same like a job approval, you have the district manager, you have the, you know, different level, we have, we just said to have different approval.

00:31:48
Different approver, yeah. I'm going to add this one. I don't want to complicate it, you know, the logic. Yeah, okay, yeah, makes sense. Because they want to have conditional approval, and then technical approval, and then final, approval, which is commercial. But one can skip, right? That's the problem, I think. I think one. Sometimes it just goes straight to commercial approval if there is no additional adjustment.

00:32:22
That totally doesn't matter. They just dropped the pass for approval. So what should be approved, you know, step by step, or it can go through the condition, go through the last, so if Jeremy is approved, it's okay. It's done. It's all, after Jeremy is approved, it goes to Jeff's approval, that's, you know, we just highlight it. Just let them know, let us know, you know, what's the condition, and we just add logic there.

00:32:56
That's, don't worry about it. It's exactly the same as the job approval, plan approval, exactly the same. So because of that, I cancel to do a wait-and-approval. I don't think we need that. since it's going to a specific person and the person doesn't want it. It's the same thing as the waiting review, right?

00:33:27
Yeah, yeah, it's the same thing. It's sent for approval, and it's waiting for approval. Yeah, that's the high-level status, but if the approval status happens, here it's waiting for approval, and they can know it's being approved by Jeremy, you know, they can see the notification. It just happens until it's, you know,

00:34:00
it's like a manager approved. Approved, we're going to need the same as... Yeah, so in the approved, I had to put in the conditional approval as a status to show that it's still conditional. No, no, the status is just waiting for approval. You don't have the status. You have separate status. Yes. It's, you know, manager approval is required.

00:34:35
But what she's saying is conditional approval is I approve this program as it is, but we may not have all the information from the client. If I get more information, I may change it. But today, with the information I have, and that's what they're kind of saying, additional approval.

00:35:11
And they want to be able to know which one is, if it's a conditional or technical approval because there was something you said about access rights. If it's conditional, some certain people won't have access to it yet. Okay. Here is the situation. If they say, I need more information from CLAN, but CLAN will give you information or they won't give you any information, that will be at least.

00:35:42
So, approved is approved, but we just added another thing is if something coming, if the CLAN has something changed, come back for change, we need to go back to the process again. Same thing if you set a conditional approval. but you know it is you know if you don't get it it will become official right but if even you say i finally approve it but clients say i have changed sorry i have changed you have to really.

00:36:15
you know start over again so it doesn't you know but but so you're right and what they're concerned about is when when it's conditional or technically approved they don't want to create call sheets from that program okay and then once it's final approval or whatever it is now it's available for call sheet creation okay it's that's kind of that's kind of a little bit of the okay that's.

00:36:48
that will be a flag they approve it and they can check mark it's a conditional or it's final. and that's just like you say. that's all that's that's not the the let's see if we talk about this is the program request we are talking about the program request so i'm not talking about the program the program we have the redis that is so called you can call it but we can announce that it's contained it's conditional.

00:37:23
and if they want to change it they want to they need one more step to say confirm you know the approval to make it ready so that's just a flag it's the same workflow you know this is just a flag they can check my flag this flag will you know affect the caution creation say if conditional, you have to confirm it before you go ahead for operation so that's a very big flag.

00:37:56
What we can show is the approval. We can show it's a question mark or it's a check mark. Question mark is conditional. Check mark is the official approval. And the workflow is the same. If the client says I have something changed, they have to come back to do another round. So in the approval, we don't want to send an email saying you've been assigned to approve this.

00:38:31
and include conditional approval, technical approval, commercial approval, and then they can select any of them. The conditional, this only happens one step, right? Whoever said that this will be the first approver will be Jeremy. Oh. Jeremy or Warren. That's the role. Whoever. Right?

00:39:02
That's the... Yeah. The approver role. Yeah. That's the department approver or something. The engineering department approver's role. That's a... He was saying, you know, is it conditional or, you know... But for the rest, it's basically the... That's the risk control for the... For Jeff or whoever. That won't be the... That won't be the case. So we will allow only him to change the check mark because he in charge of the engineering.

00:39:34
design. Yeah. And other people can see it's conditional approval. They know the risk. You know. It may change. Come back again. That's fine. Yeah. That's where we reflect. So do we want to show this as the status or we just show approval? No, no, the conditional will be one column that, you know, a flag, you can see it. This is conditional.

00:40:07
Okay, so it will show approved with a flag or approved with a check mark. Yeah, it's official, it's conditional. Basically, we just want a true for the conditional, that's not the status. That won't trigger, won't trigger anything, right? The conditional won't trigger anything. One thing it may trigger is a, you know, before it happen and then we may trigger, say, somebody.

00:40:43
we need to contact the client to confirm. You are not giving me any other change, so that's maybe the case, but that would be the flag. So, send to sales, present, release to clients, release to operations, whoever started the process, that's me.

00:41:47
Then from the logic of talk about it, it should be separate, set to sales, and when the sales are accepted, it's a waiting, pressing. And then for the sent to sales, are we sending to the whole group as well, or just the person that is assigned to sales, because sales also have assigned to it? Just two. Okay, just two. And when we separate that, it's a waiting, pressing, we accept it.

00:42:20
Sales are great, so many steps. But we just draw the happy path for here, and then if anybody not happy, and yeah, so. sent to sales, sales will get a notification and so well at this time how they get the.

00:43:06
actually the program when the when the job design done the default pricing is has been set right and, sales just review the pricing and make an adjustment and what is the tool for them to do the price this is kind of what we talked about yesterday that i need to draw up for, okay this this relates to the costing tool yeah and our printability.

00:43:42
Because doing it this way, we've kind of split the program into two pieces. We have the procedure and stuff, and we have pricing. Sales gets the pricing, they monkey with the pricing, change it. Now I've got to stick them back together and create a client-facing document. So this is where everything starts to relate. Okay, so then we will have an interface for the sales to do the pricing.

00:44:21
We need a pricing tool. And I see it as simple PCPR. Essentially calculate your PCPR, and there's a box to manipulate the price. They may want, in that calculation, we're going to need to do it two ways.

00:44:54
Price, what's my PCPR, or enter a PCPR and I calculate a price. But currently, all of the pricing is at per rate, right? They don't put a discount anymore, or they still adjust the discount? No, no. They just say, oh, here is the PCPR number, I just increase the blend cost, you know, for $100 per 10, and then to match up the, you know, expectation.

00:45:28
Discount is a result of it's not. Okay, so just send to sales, and it will be the old model. just make the two separate status is the awaiting pricing it's a oh no they have to they don't need to the sales won't say accept it and change again right send for okay that's okay it's just send.

00:45:59
sales let's make it simple and you know and then sales when sales open the pricing tool and it will change it to pricing yeah you either once that one the one more status is pricing yeah let's call it pricing and this is the the sales opened a pricing tool, we will trigger the change and then the you know from the interface they will know they.

00:46:33
are doing pricing but at this time it's out of germany's control. Once he has approved, he has done his job. It gets very muddy around here, in this area. But if the pricing is not permissive, the sales cannot reach the expectation anyway,

00:47:10
and he wants to say, hey, can you program in another way, use another brand to drop the cost? So what will happen? Progress. So, yeah, so is pricing satisfied or something?

00:47:58
So, what should be buttoned there? Request redesign or something? Yeah, I think you need two buttons. One, save. One is, yeah. Save. Yeah. Whatever. And then send back to for adjustment. Yeah, okay. You can say the buttons should be where? Save, send for adjustment.

00:48:30
This is on me to draw this up. Go back for adjustment. Okay. Send back for adjustment. Go back. Send back for adjustment.

00:49:06
or request an adjustment. Maybe not send back, just request an adjustment. So if we request an adjustment, it will go back to the engineer. So if it's a request for adjustment, do we need Jeremy to approve it, or is it just go back to the engineer directly? And then all the approval will be wiped off.

00:49:36
I think all of it has to be re-approved. Yeah, it has to restart again. Yeah, and then, you know, any adjustment, we need to follow the approval process again. So yeah, just let them know this is critical, because this... Change the blend? It's no longer approved? Yeah, basically they changed the blend.

00:50:08
The blend will affect the operation time, the cost, the material cost, labor cost, everything, right? So it should go back, yeah. No, I don't think he touched anything on the...

00:50:40
So we have a larger, we are looking at a larger scope than the engineer group, yeah. But the reason that we sort of need to do this is you have a guy like Manjeet officially in sales. yeah he does he writes the programs and he does all the pricing a guy like him.

00:51:11
can do the whole process yeah probably same thing henry on some of henry's clients he's the exact same thing yeah right any of the chinese yeah group he looks after yeah henry will be busier mark carney in china we'll see we'll see what happens, everybody's freaking out about that on the news it's a really positive because, the president of china would like to see him as an elf real chance come to see me that that should.

00:51:49
be positive if he spoiled this opportunity he'll step down you know yeah this is you know. In the past 10 years, Trudeau didn't get a chance to see the president of China. No, I'm not going to see this. Go away. You are a boy, you are a kid, and I'm not going to talk to you.

00:52:21
You are a kid. You are just a kid. It's in one meeting, it's one conference, somewhere, in Indonesia or somewhere, and just Trudeau tried to talk to the presidency, and the presidency just responded in that way. You are so naive. And then leave. I'm not going to talk to you. You are so naive.

00:52:51
Because, you know, you started, you need to start the important thing, but Justin Trudeau just started with human rights. So I'm not going to talk about this one. This is not important. For me, not important. You need to show your respect and then we can talk. If you don't show your respect, we should not talk.

00:53:22
So yeah, I expect that it will be some good. The first thing, canola oil will be exported to China. South Korea will be happy. That should work because now we're in trouble in Venezuela. Yeah. We're in trouble. But yeah, I hope someday Daniel Smith will go with the group to China.

00:53:52
and then we are going to build pipelines through U.S. and get to Seattle, the heart of China. okay continue so we can discuss if i come back to that yeah adam always laughs at me because i watch uh news and videos yeah china is on my screen because i think and then adam starts laughing.

00:54:33
i like to learn lots of stuff i don't know yeah and right now china is the major player now so, everybody's trying to be in china's camp so these two really.

00:55:04
If it is saved, that will come up a proposal document, right? So we separate the job design and the pricing, and basically we need to break down the proposal, or the program, break it down to piece by piece, and the engineer is covering the job.

00:55:35
design part, and beyond the job design part is part of the program. But at this moment, they will be merged together. So whatever the, I think from your side, they say on the sales, you need a separate program to two parts. One is the job design, another is the... The sales, all of the legal terms, you know, whatever, that would be the under control sales and pricing and the sales.

00:56:11
And then at this moment, we merged as a program. So the first one is the first piece, the engineering part, we call it the job design. Is it for the pricing? No? No, the pricing is part of the, we just call it pricing. Pricing, we just call it pricing. Okay. And here is once it's saved, the whole program will be available.

00:56:55
And the status should change when it's saved? After pricing is the, what we should call, it will be the program status. The previous one is the program request status, but it's done, the program request status is done by the, you know.

00:57:26
When it's saved, do we send it to the client or to operations, or do we do something else before we send it? Well, that's what I'm pondering through. Does sales need an approval? Let me talk to Kevin.

00:58:00
Yeah, that's separate. The engineering part is done once it's sent to sales, and the rest will be the sales decision, what they want to do. And the price, once it's saved, we need the status to say, I don't know what the terms should be. If we sit in the same page, we can't call it the price, because once the price thing is done, it's the program ready, and they can print.

00:58:57
And if they need any approval, pressing approval, and Kevin will say, oh, I want to see the program, and let's talk about it separately. Let's call it program ready. And after a bit, before we talk, they're not completed.

00:59:33
So for this one, what would be the trigger they have to do that? Yeah. Release to client, how do we know it has been? Well, I think this has to do with however we print it. Yeah. The second we push the print button, it is now an uncontrolled document. It's released to everybody.

01:00:04
Oh, okay. So that's how, once it's saved, operations. So once we print the document, we need to save it in whatever file structure that we're going to do that. Can we automatically do that? Maybe. That's actually a good question. Can we create it automatically and save it in the SharePoint folder?

01:00:41
That's one thing is we needed to build this printout, you know, with our PDF printing tool. And it can be saved in the SharePoint automatically wherever you want.

01:01:16
But continue to talk to, I don't know, sales, operations, or somebody to agree to work on the printout for Koshy's third-party service ticket. Have it out 15 times? Yeah, they just continue. Because we are going to do that, and then the operations said, you know, they want to attach the Koshy to the drop package automatically.

01:01:54
you don't want to print it out and manually attach it, okay, let's change it. So you print, you print it, you want to print it, you don't want to print it, if we do this, it can be attached automatically, the printer, but for now we can't do it, it's too old. And another, for the sales, it's not the sales, the sales doesn't care about the cost sheet, so it's the service report and the service ticket.

01:02:28
We will have better looking for these two things, just think about how we print it, what's the new layout, Lauren can, you know, help to work on it, or we have somebody to design it to be better, so we have better customer, you know, customer-facing document. Because now, the problem is they print the PDF, even download, they print the service report, attach back, and that one is not reusable, which have another process to print it again in the post-job report.

01:03:03
So, we need to end up all of this mess, one thing. Yeah, so, program ready, save the program ready. So, just back to the day process, we don't go further, so we don't creep the... Yeah, so, and you can talk to sales if they want to something continue, the workflow continue.

01:03:33
Yeah, get the idea, talk to Melanie to get the budget for us, and then we can work on it. Is it a good idea? And if it's not programming ready, it will be the end. There are no completed. Because we are the target, the first target Google engineer.

01:04:03
is end up . But this one also for sales, because they have the request from client. They monitor where they are, where the progress, right? And they can expect when they can get the program. And the two releases, it's just saved and just offhand.

01:04:40
So just scratch the two for now. OK. Then when it's ready, operation can see it from the service. So for now, we'll stop that program right here? Yeah, yeah. Let's just leave cells out of here. We can only put the pricing in scope for Phase 1.

01:05:10
That's part of Melanie's requirement. Okay, yeah. So I think that should be good. Do we want to look at this diagram, I think?

01:05:43
If you covered everything, you know, I will use AI to verify the diagram and see any discrepancy there.