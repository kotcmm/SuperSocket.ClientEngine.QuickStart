import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInboundHandlerAdapter;
import io.netty.util.ReferenceCountUtil;

/**
 * Created by caipeiyu on 16/6/4.
 */
public class ProtobufServerHandler extends ChannelInboundHandlerAdapter {

    @Override
    public void channelRead(ChannelHandlerContext ctx, Object msg) { // (2)

        try {
            DefeatMessageOuterClass.DefeatMessage in = (DefeatMessageOuterClass.DefeatMessage) msg;
           if(in.getType() == DefeatMessageOuterClass.DefeatMessage.Type.BackMessage){
               System.out.print("BackMessage:");
               System.out.print(in.getBackMessage());
               System.out.flush();
           }else if(in.getType() == DefeatMessageOuterClass.DefeatMessage.Type.CallMessage){
               System.out.print("CallMessage:");
               System.out.print(in.getCallMessage());
               System.out.flush();

               DefeatMessageOuterClass.DefeatMessage out =
                       DefeatMessageOuterClass.DefeatMessage.newBuilder()
                               .setType(DefeatMessageOuterClass.DefeatMessage.Type.BackMessage)
                               .setBackMessage(BackMessageOuterClass.BackMessage
                                       .newBuilder().setContent("Hello I from server by Java Netty").build())
                               .build();

               ctx.write(out);
               ctx.flush();
           }

        } finally {
            ReferenceCountUtil.release(msg); // (2)
        }
    }

    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) { // (4)
        // Close the connection when an exception is raised.
        cause.printStackTrace();
        ctx.close();
    }
}
